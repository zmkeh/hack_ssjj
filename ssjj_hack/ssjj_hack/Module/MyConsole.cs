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

            _ShellReadThread.Abort();
            FreeConsole();
        }

        public override void Update()
        {
            base.Update();

            Shell.Print("Updates");

        }

        private static object _cachedObj = null;
        private static Thread _ShellReadThread;
        private static string readLine = "";
        private void ShellReadThread()
        {
            while (true)
            {
                readLine = Console.ReadLine();

                if (readLine.ToLower().StartsWith("reset"))
                {
                    _cachedObj = Contexts.sharedInstance;
                    Shell.Print("Reset OK");
                    Shell.PrintObj(_cachedObj);
                }
                else if (readLine.ToLower().StartsWith("set"))
                {
                    var name = readLine.Substring(4).Trim();
                    if (_cachedObj == null)
                    {
                        Shell.Print("None Object Cached");
                    }
                    if (_cachedObj.GetType().GetField(name) != null)
                    {
                        _cachedObj = _cachedObj.GetType().GetField(name).GetValue(_cachedObj);
                        Shell.PrintObj(_cachedObj);
                    }
                    else if (_cachedObj.GetType().GetProperty(name) != null)
                    {
                        _cachedObj = _cachedObj.GetType().GetProperty(name).GetValue(_cachedObj, null);
                        Shell.PrintObj(_cachedObj);
                    }
                }
            }
        }
    }

    public static class Shell
    {
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
                if (f.IsPrivate
                    || f.FieldType == typeof(Vector2)
                    || f.FieldType == typeof(Vector3)
                    || f.FieldType == typeof(Rect)
                    || f.FieldType == typeof(Quaternion))
                {
                    Print(f.Name + " = " + f.ToString());
                }
                else
                {
                    Print(f.Name + " = " + f.FieldType.Name);
                }
            }

            Print("Fields:");
            foreach (var p in t.GetProperties())
            {
                if (p.PropertyType.IsPrimitive
                    || p.PropertyType == typeof(Vector2)
                    || p.PropertyType == typeof(Vector3)
                    || p.PropertyType == typeof(Rect)
                    || p.PropertyType == typeof(Quaternion))
                {
                    Print(p.Name + " = " + p.ToString());
                }
                else
                {
                    Print(p.Name + " = " + p.PropertyType.Name);
                }
            }

            Print("Methods:");
            foreach (var m in t.GetMethods())
            {
                var args = "";
                foreach (var arg in m.GetGenericArguments())
                {
                    args += arg.Name + ",";
                }
                if (args.Length > 0)
                    args = args.Substring(0, args.Length - 1);
                Print(m.Name + "(" + args + ")");
            }
        }

        public static void Print(string format, params object[] args)
        {
            Print(string.Format(format, args));
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
