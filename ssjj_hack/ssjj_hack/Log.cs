using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ssjj_hack
{
    public static class Log
    {
        private static string _file = null;
        public static string file
        {
            get
            {
                if (_file == null)
                {
                    var _date = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_ms");
                    _file = Application.streamingAssetsPath + $"/log.txt";
                }
                return _file;
            }
        }

        private static Queue<string> _lastLog = new Queue<string>();
        public static void OnGUI()
        {
            if (_lastLog.Count <= 0)
                return;
            if (GUI.Button(new Rect(720, 0, Screen.width - 720, 18), "Clear"))
            {
                _lastLog.Clear();
            }
            var _logs = "";
            var index = 0;
            foreach (var _log in _lastLog)
            {
                _logs += $"{++index}) {_log}\n";
            }
            GUI.Box(new Rect(720, 20, Screen.width - 720, 150), _logs);
        }

        public static void Print(string msg)
        {
            if (_lastLog.Count >= 5)
                _lastLog.Dequeue();
            _lastLog.Enqueue(msg);
            var _date = DateTime.Now.ToString("HH:mm:ss.ms");
            File.AppendAllText(file, $"[{_date}] {msg}\r\n");
        }


        private static Dictionary<string, string> logCount = new Dictionary<string, string>();
        public static void PrintOnce(string key, string msg)
        {
            if (logCount.ContainsKey(key))
                return;
            logCount[key] = msg;
            Log.Print(msg);
        }

        public static void Print(Exception ex)
        {
            Print("err: " + ex.Message + "\r\n" + ex.StackTrace);
        }
    }
}
