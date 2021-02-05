using System.Collections.Generic;
using UnityEngine;

namespace ssjj_hack
{
    public class Inspector : ModuleBase
    {
        List<Item> items = new List<Item>();
        Window window = null;
        Vector2 scroll;
        GameObject gameObject;

        public override void Awake()
        {
            base.Awake();
            window = new Window("检视面板", new Vector2(Screen.width * 0.5f, 10), new Vector2(500, 700), OnWindowGUI);
        }

        public static void SetObject(GameObject go)
        {
            Loop.GetPlugin<Inspector>()?.SetObjet(go);
        }

        protected void SetObjet(GameObject go)
        {
            this.gameObject = go;
            items.Clear();
            foreach (var comp in go.GetComponents<Component>())
            {
                items.Add(new Item(comp));
            }
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
                item.DrawUI();
            }
            GUILayout.EndScrollView();
        }

        public class Item
        {
            public Component component;
            public string name;
            public List<Item> items;

            public bool isValid => component != null;

            public Item(Component component)
            {
                this.component = component;
                this.name = component.GetType().Name;
            }

            public void DrawUI()
            {
                if (!isValid)
                {
                    GUILayout.BeginHorizontal();
                    GUI.contentColor = Color.red;
                    GUILayout.Button(name + "(deleted)", GUI.skin.label);
                    GUILayout.EndHorizontal();
                    return;
                }

                GUI.contentColor = Color.white;
                GUIPro.ObjectField(name, component, component.GetType());
            }
        }
    }
}
