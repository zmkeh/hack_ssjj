using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ssjj_hack
{
    public class Log : ModuleBase
    {
        public static bool writeToFile = false;
        public static int logStackCount = 100;

        public override void OnGUI()
        {
            wind.CallOnGUI();
        }

        private static Window wind = new Window("日志", new Vector2(Screen.width * 0.6f, 10), new Vector2(400, 400), OnWindowGUI);

        private static Queue<string> cachedLogs = new Queue<string>();
        private static int selectedLogIndex = -1;
        private static Vector2 scroll;

        private static void OnWindowGUI()
        {
            if (cachedLogs.Count <= 0)
                return;

            GUI.contentColor = Color.green;
            GUI.skin.button.alignment = TextAnchor.MiddleLeft;
            if (GUILayout.Button("清空日志", GUILayout.Width(64)))
            {
                cachedLogs.Clear();
                selectedLogIndex = -1;
            }

            var index = 0;
            var selectedLog = "";
            scroll = GUILayout.BeginScrollView(scroll, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);
            foreach (var _log in cachedLogs)
            {
                var isSelected = index == selectedLogIndex;
                var style = isSelected ? GUI.skin.textField : GUI.skin.box;
                GUI.contentColor = isSelected ? Color.green : Color.white;
                var btnLog = _log;
                if (GUILayout.Button(btnLog, style))
                {
                    selectedLogIndex = index;
                }
                if (isSelected)
                {
                    selectedLog = _log;
                }
                index++;
            }
            GUILayout.EndScrollView();
            GUI.contentColor = Color.white;
            GUILayout.TextArea(selectedLog);
        }


        public static void Print(object msg)
        {
            if (cachedLogs.Count >= logStackCount)
            {
                cachedLogs.Dequeue();
                selectedLogIndex--;
            }
            var _date = DateTime.Now.ToString("HH:mm:ss.ms");
            cachedLogs.Enqueue($"[{_date}] {msg}");
            if (writeToFile)
            {
                File.AppendAllText(file, $"[{_date}] {msg}\r\n");
            }
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
            Print("Error: " + ex.Message + "\r\n" + ex.StackTrace);
        }


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
    }
}
