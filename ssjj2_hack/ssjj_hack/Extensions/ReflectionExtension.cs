using System.Reflection;
using UnityEngine;

namespace ssjj_hack
{
    public static class ReflectionExtension
    {
        public static object ReflectMember(this object obj, string name) 
        {
            if (obj == null)
            {
                Log.PrintError("reflection obj is null.");
                return default;
            }

            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var f = obj.GetType().GetField(name, flags);
            if (f != null)
            {
                return f.GetValue(obj);
            }
            var p = obj.GetType().GetProperty(name, flags);
            if (p != null)
            {
                return p.GetValue(obj, null);
            }
            Log.PrintError($"{obj.GetType().Name} no Field named {name}.");
            return default;
        }

        public static T ReflectProperty<T>(this object obj, string name)
        {
            if (obj == null)
            {
                Log.PrintError("reflection obj is null.");
                return default(T);
            }

            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var p = obj.GetType().GetProperty(name, flags);
            if (p == null)
            {
                Log.PrintError($"{obj.GetType().Name} no Property named {name}.");
                return default(T);
            }
            return (T)p.GetValue(obj, null);
        }

        public static object ReflectProperty(this object obj, string name)
        {
            if (obj == null)
            {
                Log.PrintError("reflection obj is null.");
                return default;
            }

            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var p = obj.GetType().GetProperty(name, flags);
            if (p == null)
            {
                Log.PrintError($"{obj.GetType().Name} no Property named {name}.");
                return default;
            }
            return p.GetValue(obj, null);
        }

        public static T ReflectField<T>(this object obj, string name)
        {
            if (obj == null)
            {
                Log.PrintError("reflection obj is null.");
                return default(T);
            }

            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var f = obj.GetType().GetField(name, flags);
            if (f == null)
            {
                Log.PrintError($"{obj.GetType().Name} no Field named {name}.");
                return default(T);
            }
            return (T)f.GetValue(obj);
        }

        public static object ReflectField(this object obj, string name)
        {
            if (obj == null)
            {
                Log.PrintError("reflection obj is null.");
                return default;
            }

            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var f = obj.GetType().GetField(name, flags);
            if (f == null)
            {
                Log.PrintError($"{obj.GetType().Name} no Field named {name}.");
                return default;
            }
            return f.GetValue(obj);
        }

        public static MethodInfo ReflectMethod(this object obj, string name)
        {
            if (obj == null)
            {
                Log.PrintError("reflection obj is null.");
                return null;
            }
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var m = obj.GetType().GetMethod(name, flags);
            if (m == null)
            {
                Log.PrintError($"{obj.GetType().Name} no Method named {name}.");
                return null;
            }
            return m;
        }

        public static void ReflectInvokeMethod(this object obj, string name, params object[] parameters)
        {
            if (obj == null)
            {
                Log.PrintError("reflection obj is null.");
                return;
            }

            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var m = obj.GetType().GetMethod(name, flags);
            if (m == null)
            {
                Log.PrintError($"{obj.GetType().Name} no Method named {name}.");
                return;
            }
            m.Invoke(obj, parameters);
        }

        public static T ReflectInvokeMethod<T>(this object obj, string name, params object[] parameters)
        {
            if (obj == null)
            {
                Log.PrintError("reflection obj is null.");
                return default(T);
            }

            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var m = obj.GetType().GetMethod(name, flags);
            if (m == null)
            {
                Log.PrintError($"{obj.GetType().Name} no Method named {name}.");
                return default(T);
            }
            return (T)m.Invoke(obj, parameters);
        }
    }
}
