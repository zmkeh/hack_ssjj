using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ssjj_hack
{
    public static class Watcher
    {
        private static Dictionary<string, object> records = new Dictionary<string, object>();
        private static Vector2 scroll;
        public static void OnGUI()
        {
            if (records.Count <= 0)
                return;
            Color lastColor = GUI.contentColor;
            GUI.contentColor = Color.green;
            if (GUILayout.Button("Clear", GUILayout.Width(500)))
            {
                records.Clear();
            }
            scroll = GUILayout.BeginScrollView(scroll, GUILayout.Width(500));
            var index = 0;
            foreach (var rec in records)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"{++index}", GUILayout.Width(50));
                GUILayout.Label($"{rec.Key}", GUILayout.Width(220));
                GUILayout.Label($"{rec.Value}", GUILayout.Width(200));
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
            GUI.contentColor = lastColor;
        }

        public static void Record(string key, object value)
        {
            records[key] = value;
        }
    }
}
