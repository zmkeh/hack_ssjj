using System.Collections.Generic;
using UnityEngine;

namespace ssjj_hack
{
    public class GizmosPro : ModuleBase
    {
        private List<GizmosProGraph> graphList = new List<GizmosProGraph>();
        private const int MAX_GRAPH_COUNT = 1000;
        private static GizmosPro _ins;
        public static GizmosPro ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new GizmosPro();
                }
                return _ins;
            }
        }

        private GizmosProGraph CreateGraph(bool isRandom = false)
        {
            GizmosProGraph item = null;
            var readys = graphList.ToList(a => !a.isShow);
            if (readys.Count > 0)
            {
                int x = 0;
                if (isRandom)
                    x = UnityEngine.Random.Range(0, readys.Count);
                item = readys[x];
            }
            else if (graphList.Count <= MAX_GRAPH_COUNT)
            {
                item = new GizmosProGraph();
                graphList.Add(item);
            }
            return item;
        }

        private Dictionary<string, int> m_drawCounts = new Dictionary<string, int>();

        private static bool CheckDrawCount(string key)
        {
            if (string.IsNullOrEmpty(key))
                return true;
            int count = 0;
            if (ins.m_drawCounts.TryGetValue(key, out count))
            {
                if (count <= 0)
                    return true;
                return false;
            }
            else
            {
                ins.m_drawCounts.Add(key, 1);
                return true;
            }
        }
        public static void DrawCircle(TCircle circle, Color color, string drawOncePerFrameKey = null)
        {
            if (ins == null || !CheckDrawCount(drawOncePerFrameKey))
                return;
            var item = ins.CreateGraph();
            if (item != null)
            {
                item.Show(circle, color);
            }
        }

        public static void DrawRect(TRect rect, Color color, string drawOncePerFrameKey = null)
        {
            if (ins == null || !CheckDrawCount(drawOncePerFrameKey))
                return;
            var item = ins.CreateGraph();
            if (item != null)
            {
                item.Show(rect, color);
            }
        }

        public static void DrawEllipse(TEllipse ellipse, Color color, string drawOncePerFrameKey = null)
        {
            if (ins == null || !CheckDrawCount(drawOncePerFrameKey))
                return;
            var item = ins.CreateGraph();
            if (item != null)
            {
                item.Show(ellipse, color);
            }
        }

        public static void DrawLine(Vector2 p1, Vector2 p2, Color color, string drawOncePerFrameKey = null)
        {
            if (ins == null || !CheckDrawCount(drawOncePerFrameKey))
                return;
            var item = ins.CreateGraph();
            if (item != null)
            {
                item.Show(p1, p2, color);
            }
        }

        public void CallOnGUI()
        {
            if (ins == null)
                return;

            // update
            List<string> keys = new List<string>(ins.m_drawCounts.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                ins.m_drawCounts[keys[i]] = 0;
            }

            // draw
            for (int i = 0; i < graphList.Count; ++i)
            {
                if (graphList[i].isShow)
                {
                    graphList[i].Draw();
                }
            }
        }

        public override void OnGUI()
        {
            ins.CallOnGUI();
        }
    }
}
