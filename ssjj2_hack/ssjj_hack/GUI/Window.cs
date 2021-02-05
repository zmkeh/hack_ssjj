using System;
using UnityEngine;

namespace ssjj_hack
{
    public class Window
    {
        public static int s_focusedID = 0;
        private static int s_id = 0;

        public int id;
        public string name;
        public Rect rect;
        public Action onGUI;
        public bool isFocused => id == s_focusedID;

        public Window(string name, Vector2 position, Vector2 size, Action onGUI = null)
        {
            this.id = ++s_id;
            this.name = name;
            this.rect = new Rect(position, size);
            this.onGUI = onGUI;
        }

        public void CallOnGUI()
        {
            rect = GUI.Window(id, rect, WindowFunc, name);
            if (rect.x < 1) rect.x = 1;
            if (rect.y < 1) rect.y = 1;
            if (rect.x > Screen.width - rect.width - 1) rect.x = Screen.width - rect.width - 1;
            if (rect.y > Screen.height - rect.height - 1) rect.y = Screen.height - rect.height - 1;
        }

        //TODO: resize

        private void WindowFunc(int id)
        {
            Event e = Event.current;
            if (e.type == EventType.MouseDown ||
                e.type == EventType.MouseUp)
            {
                s_focusedID = id;
            }

            //定义窗体可以活动的范围
            GUI.DragWindow(new Rect(0, 0, rect.width, 20));
            OnGUI();
        }

        protected virtual void OnGUI()
        {
            var btnAlignment = GUI.skin.button.alignment;
            var contentColor = GUI.contentColor;

            this.onGUI?.Invoke();

            GUI.skin.button.alignment = btnAlignment;
            GUI.contentColor = contentColor;
        }
    }
}
