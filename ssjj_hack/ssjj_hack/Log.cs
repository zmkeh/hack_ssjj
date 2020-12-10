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
                    _file = Application.streamingAssetsPath + $"/log_{_date}.txt";
                }
                return _file;
            }
        }

        public static void Init()
        {
            if (!File.Exists(file))
            {
                File.Create(file).Close();
            }
        }

        public static void Print(string msg)
        {
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
            Print(ex.Message + "\r\n" + ex.StackTrace);
        }
    }
}
