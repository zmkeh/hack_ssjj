using System.Collections.Generic;
using UnityEngine;

namespace ssjj_hack
{
    public class Inspector : ModuleBase
    {
        List<ObjectViewer> items = new List<ObjectViewer>();
        Window window = null;
        Vector2 scroll;

        public override void Awake()
        {
            base.Awake();
            window = new Window("检视面板", new Vector2(480, 10), new Vector2(400, 700), OnWindowGUI);
            window.isMinimize = true;
        }

        public static void SetGameObject(GameObject go)
        {
            Loop.GetPlugin<Inspector>()?._SetGameObject(go);
        }

        public static void SetObject(object obj)
        {
            Loop.GetPlugin<Inspector>()?._SetObject(obj);
        }

        private void _SetGameObject(GameObject go)
        {
            items.Clear();
            if (go == null) return;
            foreach (var comp in go.GetComponents<Component>())
            {
                items.Add(new ObjectViewer(comp));
            }
        }

        private void _SetObject(object obj)
        {
            if (obj == null) return;
            items.Clear();
            items.Add(new ObjectViewer(obj));
        }

        public override void OnGUI()
        {
            base.OnGUI();
            window?.CallOnGUI();
        }

        private void OnWindowGUI()
        {
            scroll = GUILayout.BeginScrollView(scroll, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);
            foreach (var item in items)
            {
                item.DrawGUI();
            }
            GUILayout.EndScrollView();
        }
    }
}
