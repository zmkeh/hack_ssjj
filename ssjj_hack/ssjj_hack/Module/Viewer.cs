using Assets.Sources.Framework.System;
using System;
using System.Collections;
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
                viewObj = new ViewObj(GameModuleFeature.Instance);
            }

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
        public static BindingFlags flags => BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public object value;
        public ViewObj parent;
        public bool isFoldout;

        public Type type => _GetType();
        public string name => _GetName();
        public string typeName => _GetTypeName();
        public int depth => parent == null ? 0 : parent.depth + 1;

        public bool isField => fieldInfo != null;
        public bool isProp => propertyInfo != null;
        public bool isPrim => type != null && TypePrim(type);
        public bool isValid => type != null && !isException;
        public bool isModiable => isPrim && (isField || isProp) && parent != null;
        public bool isArrayItem => arrayIndex >= 0;

        public FieldInfo fieldInfo;
        public PropertyInfo propertyInfo;
        public int arrayIndex = -1;
        public Type arrayItemType => parent == null ? null : parent.type;
        public List<ViewObj> children = null;
        public bool isException = false;
        public string exMsg = "";

        private Type _GetType()
        {
            if (value != null)
                return value.GetType();
            if (isArrayItem)
                return arrayItemType;
            if (isField)
                return fieldInfo.FieldType;
            if (isProp)
                return propertyInfo.PropertyType;
            return null;
        }

        private string _GetName()
        {
            if (isArrayItem)
                return $"item{arrayIndex:D3}";
            if (isField)
                return fieldInfo.Name;
            if (isProp)
                return propertyInfo.Name;
            if (value != null)
                return value.GetType().Name;
            return "null";
        }

        private string _GetTypeName()
        {
            if (type.Name == "List`1")
            {
               return $"List<{type.GetGenericArguments()[0].Name}";
            }
            return type.Name;
        }

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
                GUI.Box(rect, " " + name + " -> Exception: " + exMsg);
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
                if (GUI.Button(rect, (isFoldout ? "▼" : "▶") + name + "(" + typeName + ")"))
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

        public ViewObj(object obj)
        {
            value = obj;
            Reset();
        }

        public ViewObj(FieldInfo info, ViewObj parent)
        {
            this.fieldInfo = info;
            this.parent = parent;
            Reset();
        }

        public ViewObj(PropertyInfo info, ViewObj parent)
        {
            this.propertyInfo = info;
            this.parent = parent;
            Reset();
        }

        public ViewObj(int arrayIndex, ViewObj parent)
        {
            this.arrayIndex = arrayIndex;
            this.parent = parent;
            Reset();
        }

        public void Reset()
        {
            try
            {
                if (isArrayItem)
                {
                    value = (parent.value as IList)[arrayIndex];
                }
                else if (isField)
                {
                    value = fieldInfo.GetValue(parent.value);
                }
                else if (isProp)
                {
                    value = propertyInfo.GetValue(parent.value, null);
                }
            }
            catch (Exception ex)
            {
                exMsg = ex.Message;
                isException = true;
            }
        }

        public void GetChildren()
        {
            if (children == null)
            {
                children = new List<ViewObj>();
                if (type.IsArray && value != null)
                {
                    var lst = value as IList;
                    for (int i = 0; i < lst.Count; i++)
                    {
                        children.Add(new ViewObj(i, this));
                    }
                }
                else
                {
                    foreach (var info in type.GetFields(flags))
                    {
                        children.Add(new ViewObj(info, this));
                    }

                    foreach (var info in type.GetProperties(flags))
                    {
                        children.Add(new ViewObj(info, this));
                    }
                }
                children.Sort((a, b) => (a.isValid != b.isValid) ? b.isValid.CompareTo(a.isValid) : (a.isPrim != b.isPrim) ? a.isPrim.CompareTo(b.isPrim) : a.name.CompareTo(b.name));
            }
            else
            {
                foreach (var c in children)
                {
                    c.Reset();
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
    }
}
