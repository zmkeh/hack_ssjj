using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ssjj_hack.Module
{
    public class Viewer : ModuleBase
    {
        public override void Awake()
        {
            base.Awake();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        private ViewObj viewObj = null;
        private Vector2 scroll = Vector2.zero;

        public override void OnGUI()
        {
            base.OnGUI();
            GUI.contentColor = Color.green;
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            GUI.skin.box.alignment = TextAnchor.MiddleLeft;
            GUI.skin.button.alignment = TextAnchor.MiddleLeft;

            if (GUILayout.Button("Refresh"))
            {
                viewObj = new ViewObj(Contexts.sharedInstance.player);
            }

            // Contexts.sharedInstance.player.lastWeapon.

            if (viewObj != null)
            {
                scroll = GUILayout.BeginScrollView(scroll);
                viewObj.DrawGUI();
                GUILayout.EndScrollView();
            }

            scroll += Input.mouseScrollDelta.y * Vector2.up * 0.1f;
        }
    }

    public static class Sim
    {
        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        //移动鼠标 
        const int MOUSEEVENTF_MOVE = 0x0001;

        public static void Move(Vector2 delta)
        {
            mouse_event(MOUSEEVENTF_MOVE, (int)delta.x, (int)-delta.y, 0, 0);
        }
    }

    public class ViewObj
    {
        public bool isFoldout { get; set; }

        public string name;
        public object value;
        public ViewObj parent;
        public int depth;

        public bool isField => fieldInfo != null;
        public bool isPrim => type != null && TypePrim(type);
        public bool isValid => type != null && !isException;
        public Type type => value != null ? value.GetType() : fieldInfo != null ? fieldInfo.FieldType : propertyInfo != null ? propertyInfo.PropertyType : null;
        public static BindingFlags flags => BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public FieldInfo fieldInfo;
        public PropertyInfo propertyInfo;
        public List<ViewObj> children = null;
        public bool isException = false;

        public Rect GetRect()
        {
            float width = 700;
            float height = 20;
            var rect = GUILayoutUtility.GetRect(width, height);
            rect.width = width - depth * 20 - 16;
            rect.x = depth * 20;
            return rect;
        }

        private void SetValue(object val)
        {
            if (fieldInfo != null)
                fieldInfo.SetValue(parent.value, val);
            else if (propertyInfo != null)
                propertyInfo.SetValue(parent.value, val, null);
            value = val;
        }

        public void DrawGUI()
        {
            var rect = GetRect();

            if (isException)
            {
                GUI.contentColor = Color.red;
                GUI.Box(rect, " " + name + " -> Exception");
                return;
            }

            if (!isValid)
            {
                GUI.contentColor = Color.red;
                GUI.Box(rect, " " + name + " -> not valid");
                return;
            }

            if (isPrim)
            {
                GUI.contentColor = Color.white;

                var rect1 = new Rect(rect);
                rect1.width = 300;
                GUI.Box(rect, " " + name + " = ");
                var rect2 = new Rect(rect);
                rect2.width = rect.width - rect1.width - 10;
                rect2.x = rect1.x + rect1.width + 10;

                if (type == typeof(bool))
                {
                    var val = GUI.Toggle(rect2, (bool)value, "");
                    if (val != (bool)value)
                    {
                        SetValue(val);
                    }
                }
                else if (type == typeof(int))
                {
                    if (int.TryParse(GUI.TextField(rect2, value.ToString()), out var val) && val != (int)value)
                    {
                        SetValue(val);
                    }
                }
                else if (type == typeof(float))
                {
                    if (float.TryParse(GUI.TextField(rect2, value.ToString()), out var val) && val != (float)value)
                    {
                        SetValue(val);
                    }
                }
                else if (type == typeof(long))
                {
                    if (long.TryParse(GUI.TextField(rect2, value.ToString()), out var val) && val != (long)value)
                    {
                        SetValue(val);
                    }
                }
                else if (type == typeof(string))
                {
                    var _val = "" + value;
                    var val = GUI.TextField(rect2, _val);
                    if (value == null && !string.IsNullOrEmpty(val) || value != null && value.ToString() != val)
                    {
                        SetValue(val);
                    }
                }
                else if (type == typeof(Vector2))
                {
                    GUI.TextField(rect2, value.ToString());
                }
                else if (type == typeof(Vector3))
                {
                    GUI.TextField(rect2, value.ToString());
                }
                else
                {
                    GUI.TextField(rect2, value.ToString());
                }
            }
            else
            {
                GUI.contentColor = Color.green;
                if (GUI.Button(rect, (isFoldout ? "▼" : "▶") + name + "(" + type.Name + ")"))
                {
                    isFoldout = !isFoldout;
                    if (!isFoldout)
                    {
                        children = null;
                    }
                }
                if (isFoldout)
                {
                    GetChildren();

                    foreach (var c in children)
                    {
                        c.DrawGUI();
                    }
                }
            }
        }

        public void Reset(FieldInfo info, ViewObj parent)
        {
            this.fieldInfo = info;
            this.parent = parent;
            this.name = info.Name;
            try
            {
                value = info.GetValue(parent.value);
            }
            catch
            {
                isException = true;
            }
        }
        public void Reset(PropertyInfo info, ViewObj parent)
        {
            this.propertyInfo = info;
            this.parent = parent;
            this.name = info.Name;
            try
            {
                value = info.GetValue(parent.value, null);
            }
            catch
            {
                isException = true;
            }
        }

        public ViewObj(object obj)
        {
            this.value = obj;
            this.depth = 0;
        }

        public ViewObj(FieldInfo info, ViewObj parent)
        {
            Reset(info, parent);
            Init();
        }

        public ViewObj(PropertyInfo info, ViewObj parent)
        {
            Reset(info, parent);
            Init();
        }

        private void Init()
        {
            this.depth = parent.depth + 1;
        }

        public void GetChildren()
        {
            if (children == null)
            {
                children = new List<ViewObj>();

                foreach (var info in type.GetFields(flags))
                {
                    children.Add(new ViewObj(info, this));
                }

                foreach (var info in type.GetProperties(flags))
                {
                    children.Add(new ViewObj(info, this));
                }
                children.Sort((a, b) => (a.isValid != b.isValid) ? b.isValid.CompareTo(a.isValid) : (a.isPrim != b.isPrim) ? a.isPrim.CompareTo(b.isPrim) : a.name.CompareTo(b.name));
            }
            else
            {
                foreach (var c in children)
                {
                    if (c.isField)
                    {
                        c.Reset(type.GetField(c.name, flags), this);
                    }
                    else
                    {
                        c.Reset(type.GetProperty(c.name, flags), this);
                    }
                }
            }
        }

        public static bool TypePrim(Type t)
        {
            return t.IsPrimitive
                        || t == typeof(string)
                        || t == typeof(Vector2)
                        || t == typeof(Vector3)
                        || t == typeof(Rect)
                        || t == typeof(Quaternion);
        }

        public static bool TypeValid(Type t)
        {
            if (t.IsArray)
                return false;
            if (t.IsGenericType)
            {
                if (t.Name.StartsWith("List`"))
                    return false;
                else if (t.Name.StartsWith("Dictionary`"))
                    return false;
                else if (t.Name.StartsWith("Queue`"))
                    return false;
                else if (t.Name.StartsWith("CircularBuffer`"))
                    return false;
            }
            return true;
        }
    }
}
