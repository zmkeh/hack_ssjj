using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace ssjj_hack
{

    public class ObjectViewer
    {
        public static BindingFlags flags => BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public object value;
        public ObjectViewer parent;
        public bool isFoldout;

        public Type type => _GetType();
        public string name => _GetName();
        public string typeName => _GetTypeName();
        public int indent => parent == null ? 0 : parent.indent + 1;

        public bool isField => fieldInfo != null;
        public bool isProp => propertyInfo != null;
        public bool isPrim => type != null && TypePrim(type);
        public bool isValid => type != null && !isException;
        public bool isArrayItem => arrayIndex >= 0;

        public FieldInfo fieldInfo;
        public PropertyInfo propertyInfo;
        public int arrayIndex = -1;
        public Type arrayItemType => parent == null ? null : parent.type;
        public List<ObjectViewer> children = null;
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
            var _typeName = _GetTypeName();
            if (type != null && !type.IsPrimitive)
                _typeName = type.FullName;
            if (isArrayItem)
                return $"item{arrayIndex:D3}";
            if (isField)
                return fieldInfo.Name + "[F]" + $"({_typeName})";
            if (isProp)
                return propertyInfo.Name + "[P]" + $"({_typeName})";
            if (value != null)
                return value.GetType().Name + "[T]" + $"({_typeName})";
            return "null";
        }

        private string _GetTypeName()
        {
            if (type == null)
            {
                return "Type NULL";
            }

            if (type.Name == "List`1")
            {
                return $"List<{type.GetGenericArguments()[0].Name}>";
            }
            return type.Name;
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
            var contentColor = GUI.contentColor;

            if (isException)
            {
                GUI.contentColor = Color.red;
                GUIPro.BeginIndent(indent);
                GUIPro.Label(" " + name + " -> Exception: " + exMsg);
                GUIPro.EndIndent();
                GUI.contentColor = contentColor;
                return;
            }

            if (!isValid)
            {
                GUI.contentColor = Color.red;
                GUIPro.BeginIndent(indent);
                GUIPro.Label(" " + name + " -> not valid");
                GUIPro.EndIndent();
                GUI.contentColor = contentColor;
                return;
            }

            if (isPrim)
            {
                GUI.contentColor = Color.white;
                GUIPro.BeginIndent(indent);
                if (type == typeof(bool))
                {
                    var val = GUIPro.Toggle(name, (bool)value);
                    if (val != (bool)value)
                    {
                        SetValue(val);
                    }
                }
                else if (type == typeof(int))
                {
                    var val = GUIPro.Int(name, (int)value);
                    if (val != (int)value)
                    {
                        SetValue(val);
                    }
                }
                else if (type == typeof(float))
                {
                    var val = GUIPro.Float(name, (float)value);
                    if (!Mathf.Approximately(val, (float)value))
                    {
                        SetValue(val);
                    }
                }
                else if (type == typeof(long))
                {
                    var val = GUIPro.Long(name, (long)value);
                    if (val != (long)value)
                    {
                        SetValue(val);
                    }
                }
                else if (type == typeof(string))
                {
                    var val = GUIPro.Text(name, value == null ? "#NULL" : value.ToString());
                    if (value == null && val != "#NULL"
                        || value != null && value.ToString() != val)
                    {
                        SetValue(val);
                    }
                }
                else if (type == typeof(Vector2))
                {
                    var val = GUIPro.Vector2(name, (Vector2)value);
                    if (val != (Vector2)value)
                    {
                        SetValue(val);
                    }
                }
                else if (type == typeof(Vector3))
                {
                    var val = GUIPro.Vector3(name, (Vector3)value);
                    if (val != (Vector3)value)
                    {
                        SetValue(val);
                    }
                }
                else
                {
                    GUIPro.Text(name, value.ToString());
                }
                GUIPro.EndIndent();
                GUI.contentColor = contentColor;
            }
            else
            {
                GUI.contentColor = Color.white;
                GUIPro.BeginIndent(indent);
                isFoldout = GUIPro.Foldout(name, isFoldout);
                GUIPro.EndIndent();
                GUI.contentColor = contentColor;

                if (isFoldout)
                {
                    GetChildren();

                    foreach (var c in children)
                    {
                        c.DrawGUI();
                    }
                }
                else
                {
                    children = null;
                }
            }
        }

        public ObjectViewer(object obj)
        {
            value = obj;
            Reset();
        }

        private ObjectViewer(FieldInfo info, ObjectViewer parent)
        {
            this.fieldInfo = info;
            this.parent = parent;
            Reset();
        }

        private ObjectViewer(PropertyInfo info, ObjectViewer parent)
        {
            this.propertyInfo = info;
            this.parent = parent;
            Reset();
        }

        private ObjectViewer(int arrayIndex, ObjectViewer parent)
        {
            this.arrayIndex = arrayIndex;
            this.parent = parent;
            Reset();
        }

        private void Reset()
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

        private void GetChildren()
        {
            if (children == null)
            {
                children = new List<ObjectViewer>();
                if (type.IsArray && value != null)
                {
                    var lst = value as IList;
                    for (int i = 0; i < lst.Count; i++)
                    {
                        children.Add(new ObjectViewer(i, this));
                    }
                }
                else
                {
                    foreach (var info in type.GetFields(flags))
                    {
                        children.Add(new ObjectViewer(info, this));
                    }

                    foreach (var info in type.GetProperties(flags))
                    {
                        children.Add(new ObjectViewer(info, this));
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

        private static bool TypePrim(Type t)
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
