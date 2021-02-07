using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ssjj_hack
{
    public class Watcher : ModuleBase
    {
        private static Dictionary<string, RecordTicks> records = new Dictionary<string, RecordTicks>();

        private static Vector2 scroll;
        private static Window window = new Window("监测", new Vector2(Screen.width - 1120, 10), new Vector2(700, 400), OnWindowGUI);


        public class RecordTicks
        {
            public long min = long.MaxValue;
            public long max;
            public long current;
            public long total;
            public long count;
            public long avg => total / count;

            public void SetCurrent(long val)
            {
                this.current = val;
                this.min = Math.Min(min, val);
                this.max = Math.Max(max, val);
                this.total += val;
                this.count++;
            }
        }

        public override void OnGUI()
        {
            base.OnGUI();
            window.CallOnGUI();
        }

        public static void OnWindowGUI()
        {
            if (records.Count <= 0)
                return;
            Color contentColor = GUI.contentColor;
            GUI.contentColor = Color.green;
            if (GUILayout.Button("清空记录", GUILayout.Width(64)))
            {
                records.Clear();
            }
            scroll = GUILayout.BeginScrollView(scroll);
            GUI.contentColor = Color.white;
            var index = 0;
            foreach (var kv in records)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"{++index}.", GUILayout.Width(20));
                GUILayout.Label($"{kv.Key}", GUILayout.Width(200));
                GUILayout.Label($"{kv.Value.current}", GUILayout.Width(60));
                GUILayout.Label($"{kv.Value.min}", GUILayout.Width(60));
                GUILayout.Label($"{kv.Value.max}", GUILayout.Width(60));
                GUILayout.Label($"{kv.Value.avg}", GUILayout.Width(60));
                GUILayout.Label($"{kv.Value.count}", GUILayout.Width(60));
                GUILayout.Label($"{kv.Value.total}", GUILayout.Width(100));
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
            GUI.contentColor = contentColor;
        }

        public static void Record(string key, long ticks)
        {
            if (!records.ContainsKey(key))
            {
                records.Add(key, new RecordTicks());
            }

            records[key].SetCurrent(ticks);
        }
    }
}
