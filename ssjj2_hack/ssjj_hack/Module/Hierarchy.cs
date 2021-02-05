using System.Collections.Generic;
using UnityEngine;

namespace ssjj_hack
{
    public class Hierarchy : ModuleBase
    {
        List<Item> rootItems = new List<Item>();
        Window window = null;
        Vector2 scroll;

        public override void Awake()
        {
            base.Awake();
            window = new Window("层级结构", new Vector2(Screen.width * 0.1f, 10), new Vector2(500, 700), OnWindowGUI);
        }

        public override void OnGUI()
        {
            base.OnGUI();
            window?.CallOnGUI();
        }

        private void OnWindowGUI()
        {
            if (GUILayout.Button("获取根节点", GUILayout.Width(80)))
            {
                rootItems.Clear();
                foreach (Transform t in UnityEngine.Object.FindObjectsOfType<Transform>())
                {
                    if (t.parent == null)
                    {
                        rootItems.Add(new Item(t));
                    }
                }
            }

            scroll = GUILayout.BeginScrollView(scroll, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);
            foreach (var item in rootItems)
            {
                item.DrawUI();
            }
            GUILayout.EndScrollView();
        }

        public class Item
        {
            public static Item selectedItem;

            public Transform transform;
            public bool isFoldout;
            public int indent;
            public string name;
            public List<Item> items;

            public bool isValid => transform != null;
            public bool isSelected => this == selectedItem;

            public Item(Transform transform, int indent = 0)
            {
                this.transform = transform;
                this.name = transform.name;
                this.indent = indent;
            }

            public void DrawUI()
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("", GUILayout.Width(indent * 16));
                if (!isValid)
                {
                    GUI.contentColor = Color.red;
                    GUILayout.Button(name + "(deleted)", GUI.skin.label);
                    GUILayout.EndHorizontal();
                    return;
                }

                GUI.contentColor = Color.white;
                var style = isSelected ? GUI.skin.textField : GUI.skin.label;
                if (transform.childCount > 0)
                {
                    if (GUILayout.Button((isFoldout ? "▼" : "▶"), GUI.skin.label, GUILayout.Width(14)))
                    {
                        isFoldout = !isFoldout;
                    }
                }

                if (GUILayout.Button(name, style))
                {
                    selectedItem = this;
                    Inspector.SetObject(this.transform.gameObject);
                }
                GUILayout.EndHorizontal();

                if (isFoldout)
                {
                    if (items == null)
                    {
                        items = new List<Item>();
                        for (int i = 0; i < transform.childCount; i++)
                        {
                            items.Add(new Item(transform.GetChild(i), indent + 1));
                        }
                    }

                    for (int i = 0; i < items.Count; i++)
                    {
                        items[i].DrawUI();
                    }
                }
            }
        }
    }
}
