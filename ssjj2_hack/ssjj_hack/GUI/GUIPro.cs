using System;
using System.Collections.Generic;
using UnityEngine;

namespace ssjj_hack
{
    public class GUIPro
    {
        public static void BeginIndent(int indent)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("", GUILayout.Width(indent * 16));
        }

        public static void EndIndent()
        {
            GUILayout.EndHorizontal();
        }

        public static bool Foldout(string name, bool isFoldout, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button((isFoldout ? "▼" : "▶"), GUI.skin.label, GUILayout.Width(14)))
            {
                isFoldout = !isFoldout;
            }
            GUILayout.Label(name);
            GUILayout.EndHorizontal();
            return isFoldout;
        }

        public static void Label(string val, params GUILayoutOption[] options)
        {
            GUILayout.Label(val, options);
        }

        public static Enum EnumPopup(Enum value, params GUILayoutOption[] options)
        {
            return (Enum)Enum.Parse(value.GetType(), GUILayout.TextField(value.ToString(), options));
        }

        public static bool Toggle(bool value, params GUILayoutOption[] options)
        {
            return GUILayout.Toggle(value, "", options);
        }

        public static float Float(float value, params GUILayoutOption[] options)
        {
            return float.Parse(GUILayout.TextField(value.ToString(), options));
        }

        public static int Int(int value, params GUILayoutOption[] options)
        {
            return int.Parse(GUILayout.TextField(value.ToString(), options));
        }

        public static long Long(long value, params GUILayoutOption[] options)
        {
            return long.Parse(GUILayout.TextField(value.ToString(), options));
        }

        public static float Float(string name, float value, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(name);
            var result = float.Parse(GUILayout.TextField(value.ToString(), options));
            GUILayout.EndHorizontal();
            return result;
        }

        public static int Int(string name, int value, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(name);
            var result = int.Parse(GUILayout.TextField(value.ToString(), options));
            GUILayout.EndHorizontal();
            return result;
        }

        public static long Long(string name, long value, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(name);
            var result = long.Parse(GUILayout.TextField(value.ToString(), options));
            GUILayout.EndHorizontal();
            return result;
        }

        public static bool Toggle(string name, bool value, params GUILayoutOption[] options)
        {
            var result = GUILayout.Toggle(value, name, options);
            return result;
        }

        public static string Text(string name, string value, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(name);
            var result = GUILayout.TextField(value, options);
            GUILayout.EndHorizontal();
            return result;
        }

        public static Vector2 Vector2(string name, Vector2 value, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(name);
            var x = Float(value.x, options);
            var y = Float(value.y, options);
            var result = new Vector2(x, y);
            GUILayout.EndHorizontal();
            return result;
        }

        public static Vector3 Vector3(string name, Vector3 value, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(name);
            var x = Float(value.x, options);
            var y = Float(value.y, options);
            var z = Float(value.z, options);
            var result = new Vector3(x, y, z);
            GUILayout.EndHorizontal();
            return result;
        }

        /*
        private static Dictionary<string, bool> foldOutObj = new Dictionary<string, bool>();
        private static object ListField(string title, object obj, Type objType, string path = "",
            Func<Type, bool> specialFilter = null, Func<string, object, Type, string, object> specialHandle = null)
        {
            int count = 0;
            if (obj != null)
                count = Convert.ToInt32(objType.GetProperty("Count").GetValue(obj, null));
            indentLevel++;
            var newCount = (int)Obj_Primary("Size", count);
            indentLevel--;
            if (newCount != count)
            {
                // add
                if (count == 0)
                {
                    obj = Activator.CreateInstance(objType);
                }
                for (int i = count; i < newCount; i++)
                {
                    var newItem = Activator.CreateInstance(objType.GetGenericArguments()[0]);
                    objType.GetMethod("Add").Invoke(obj, new object[] { newItem });
                }
                for (int i = newCount; i < count; i++)
                {
                    objType.GetMethod("RemoveAt").Invoke(obj, new object[] { i });
                }
                return obj;
            }

            indentLevel++;
            for (int i = 0; i < count; i++)
            {
                var item = objType.GetProperty("Item").GetValue(obj, new object[] { i });
                var newItem = _ObjectField("Element " + i, item, item.GetType(), path, specialFilter, specialHandle);
                if (!newItem.Equals(item))
                {
                    objType.GetProperty("Item").SetValue(obj, newItem, new object[] { i });
                }
            }
            indentLevel--;
            return obj;
        }

        private static int fieldIndex = 0;

        public static object ObjectField(string title, object obj, Type objType, string path = "",
            Func<Type, bool> specialFilter = null, Func<string, object, Type, string, object> specialHandle = null)
        {
            return _ObjectField(title, obj, objType, path, specialFilter, specialHandle);
        }

        private static int indentLevel = 0;
        private static object _ObjectField(string title, object obj, Type objType, string path,
            Func<Type, bool> specialFilter, Func<string, object, Type, string, object> specialHandle)
        {
            if (!string.IsNullOrEmpty(path))
                path = path + "/" + title;
            else
                path = title;

            // title = title + $"[{path}]";

            if (specialFilter != null && specialHandle != null && specialFilter(objType))
            {
                return specialHandle(title, obj, objType, path);
            }

            // primary

            if (objType == typeof(string)
                || objType == typeof(short)
                || objType == typeof(int)
                || objType == typeof(long)
                || objType == typeof(float)
                || objType == typeof(Vector2)
                || objType == typeof(Vector3)
                || objType == typeof(bool)
                || objType.IsEnum)
                return Obj_Primary(title, obj);

            if (!Obj_FoldOut(path, title))
                return obj;

            // list

            if (objType.IsGenericType && objType.Name == "List`1")
                return ListField(title, obj, objType, path, specialFilter, specialHandle);

            // object 

            indentLevel++;

            var newObj = obj == null ? Activator.CreateInstance(objType) : obj;

            foreach (var fi in objType.GetFields())
            {
                if (fi.IsDefined(typeof(NonSerializedAttribute), false))
                    continue;
                var fieldValue = fi.GetValue(newObj);
                if (fieldValue == null)
                    continue;
                var filedType = fieldValue.GetType();
                var val = _ObjectField(fi.Name, fieldValue, filedType, path, specialFilter, specialHandle);
                fi.SetValue(newObj, val);
            }

            indentLevel--;

            return newObj;
        }

        private const int Obj_Content_Width_ORG = 260;

        private static float Obj_GetContentWidth()
        {
            return Obj_Content_Width_ORG + indentLevel * 15;
        }

        private static GUILayoutOption Obj_GetContentOption()
        {
            return GUILayout.Width(Obj_GetContentWidth());
        }

        private static bool Obj_FoldOut(string key, string title)
        {
            if (!foldOutObj.ContainsKey(key))
                foldOutObj.Add(key, true);
            foldOutObj[key] = Foldout(title, foldOutObj[key]);
            return foldOutObj[key];
        }

        private static object Obj_Primary(string title, object obj)
        {
            GUILayout.BeginHorizontal();

            var objType = obj.GetType();
            if (objType == typeof(string))
                obj = Obj_Text(title, (string)obj);
            else if (objType == typeof(short))
                obj = (short)Obj_Int(title, (short)obj);
            else if (objType == typeof(int))
                obj = Obj_Int(title, (int)obj);
            else if (objType == typeof(long))
                obj = Obj_Long(title, (long)obj);
            else if (objType == typeof(float))
                obj = Obj_Float(title, (float)obj);
            else if (objType == typeof(Vector2))
                obj = Obj_Vector2(title, (Vector2)obj);
            else if (objType == typeof(Vector3))
                obj = Obj_Vector3(title, (Vector3)obj);
            else if (objType == typeof(bool))
                obj = Obj_Bool(title, (bool)obj);
            else if (objType.IsEnum)
                obj = Obj_Enum(title, (Enum)obj);
            else
                throw new ArgumentException(" Not a Primary Value => " + obj);

            GUILayout.EndHorizontal();
            return obj;
        }

        private static string Obj_Text(string title, string content)
        {
            Label(title);
            content = GUILayout.TextField(content, Obj_GetContentOption());
            return content;
        }

        private static int Obj_Int(string title, int val)
        {
            Label(title);
            val = Int(val, Obj_GetContentOption());
            return val;
        }

        private static long Obj_Long(string title, long val)
        {
            Label(title);
            val = Long(val, Obj_GetContentOption());
            return val;
        }

        private static float Obj_Float(string title, float val)
        {
            Label(title);
            val = Float(val, Obj_GetContentOption());
            return val;
        }

        private static bool Obj_Bool(string title, bool val)
        {
            Label(title);
            val = Toggle(val, Obj_GetContentOption());
            return val;
        }

        private static Enum Obj_Enum(string title, Enum val)
        {
            Label(title);
            val = EnumPopup(val, Obj_GetContentOption());
            return val;
        }

        private static Vector2 Obj_Vector2(string title, Vector2 val)
        {
            Label(title);
            var width = Obj_GetContentWidth() - 3 * 2;
            var lw = 20;
            var tw = (width - lw) * 0.5f;
            val.x = Float(val.x, GUILayout.Width(tw));
            Label(",", GUILayout.Width(lw));
            val.y = Float(val.y, GUILayout.Width(tw));
            return val;
        }

        private static Vector3 Obj_Vector3(string title, Vector3 val)
        {
            Label(title);
            var width = Obj_GetContentWidth() - 3 * 4;
            var lw = 20;
            var tw = (width - lw * 2) / 3;
            val.x = Float(val.x, GUILayout.Width(tw));
            Label(",", GUILayout.Width(lw));
            val.y = Float(val.y, GUILayout.Width(tw));
            Label(",", GUILayout.Width(lw));
            val.z = Float(val.z, GUILayout.Width(tw));
            return val;
        }
        */
    }
}
