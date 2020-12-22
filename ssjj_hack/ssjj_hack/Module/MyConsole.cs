using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using UnityEngine;

namespace ssjj_hack.Module
{
    public class MyConsole : ModuleBase
    {
        [DllImport("kernel32.dll")]
        public static extern Boolean AllocConsole();
        [DllImport("kernel32.dll")]
        public static extern Boolean FreeConsole();

        public override void Awake()
        {
            base.Awake();

            AllocConsole();
            _ShellReadThread = new Thread(new ThreadStart(ShellReadThread));
            _ShellReadThread.Start();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            isStop = true;
            FreeConsole();
        }

        public override void Update()
        {
            base.Update();
        }

        private static Stack<object> _cachedObjStack = new Stack<object>();
        private static object _cachedObj = null;
        private static Thread _ShellReadThread;
        private static string readLine = "";
        private static bool isStop = false;
        private void ShellReadThread()
        {
            while (!isStop)
            {
                try
                {
                    readLine = Console.ReadLine().ToLower();

                    if (_cachedObj == null)
                    {
                        _cachedObj = Contexts.sharedInstance;
                        Shell.PrintObj(_cachedObj);
                    }
                    else if (readLine == ".." && _cachedObjStack.Count > 0)
                    {
                        _cachedObj = _cachedObjStack.Pop();
                        Shell.PrintObj(_cachedObj);
                    }
                    else
                    {
                        object obj = null;
                        foreach (var f in _cachedObj.GetType().GetFields())
                        {
                            if (Shell.IsPrimType(f.FieldType) || Shell.IsList(f.FieldType))
                                continue;
                            var val = f.GetValue(_cachedObj);
                            if (val == null)
                                continue;
                            if (f.Name.ToLower().Contains(readLine))
                                obj = val;
                        }

                        if (obj == null)
                        {
                            foreach (var p in _cachedObj.GetType().GetProperties())
                            {
                                if (Shell.IsPrimType(p.PropertyType) || Shell.IsList(p.PropertyType))
                                    continue;
                                var val = p.GetValue(_cachedObj, null);
                                if (val == null)
                                    continue;
                                if (p.Name.ToLower().Contains(readLine))
                                    obj = val;
                            }
                        }

                        if (obj != null)
                        {
                            _cachedObjStack.Push(_cachedObj);
                            _cachedObj = obj;
                            Shell.PrintObj(_cachedObj);
                        }
                        else
                        {
                            Shell.Print($"can't find {readLine}");
                        }
                    }
                }
                catch (Exception e)
                {
                    Shell.Print(e);
                }
            }
        }
    }

    public static class Shell
    {
        public static bool IsPrimType(Type t)
        {
            return t.IsPrimitive
                        || t == typeof(string)
                        || t == typeof(Vector2)
                        || t == typeof(Vector3)
                        || t == typeof(Rect)
                        || t == typeof(Quaternion);
        }

        public static bool IsList(Type t)
        {
            return t.IsArray || t.IsGenericType;
        }

        public static void PrintObj(object obj)
        {
            if (obj == null)
            {
                Print("NULL");
                return;
            }

            var t = obj.GetType();

            Print("Fields:");
            foreach (var f in t.GetFields())
            {
                if (IsList(f.FieldType))
                    continue;

                if (IsPrimType(f.FieldType))
                {
                    var val = f.GetValue(obj);
                    Print($"{f.Name} = {val}");
                }
                else
                {
                    Print($"{f.Name} ({f.FieldType.Name})");
                }
            }

            Print("Props:");
            foreach (var p in t.GetProperties())
            {
                if (IsList(p.PropertyType))
                    continue;

                if (IsPrimType(p.PropertyType))
                {
                    var val = p.GetValue(obj, null);
                    Print($"{p.Name} = {val}");
                }
                else
                {
                    Print($"{p.Name} ({p.PropertyType.Name})");
                }
            }
        }

        public static void Print(string format, params object[] args)
        {
            Print(string.Format(format, args));
        }

        public static void Print(Exception e)
        {
            Print(string.Format("{0}\n{1}", e.Message, e.StackTrace));
        }

        public static void Print(string output)
        {
            Console.WriteLine(@"[{0}] {1}", DateTimeOffset.Now.ToString("HH:mm:ss.ms"), output);
        }

        public static string Read()
        {
            return Console.ReadLine();
        }
    }
}