using ssjj_hack.Module;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace ssjj_hack
{
    public class Loop : MonoBehaviour
    {
        private static Loop ins = null;

        void Awake()
        {
            try
            {
                DontDestroyOnLoad(this);
                InitPlugins();
            }
            catch (Exception ex)
            {
                Log.Print(ex);
            }

            foreach (var t in modules)
            {
                try
                {
                    t.Value.Awake();
                }
                catch (Exception ex)
                {
                    Log.Print(ex);
                }
            }
        }

        void Start()
        {
            foreach (var t in modules)
            {
                try
                {
                    t.Value.Start();
                }
                catch (Exception ex)
                {
                    Log.Print(ex);
                }
            }
        }

        void OnGUI()
        {
            foreach (var t in modules)
            {
                try
                {
                    BeginWatch(t.Key.Name + " OnGUI");
                    t.Value.OnGUI();
                    EndWatch();
                }
                catch (Exception ex)
                {
                    Log.Print(ex);
                }
            }

            Log.OnGUI();
            // Watcher.OnGUI();
            GizmosPro.OnGUI();
        }

        private Stopwatch watch = new Stopwatch();
        private string _currentWatchName = "";
        private void BeginWatch(string name)
        {
            _currentWatchName = name;
            watch.Reset();
            watch.Start();
        }

        private void EndWatch()
        {
            Watcher.Record($"{_currentWatchName}", watch.ElapsedTicks);
            watch.Stop();
        }

        void Update()
        {
            foreach (var t in modules)
            {
                try
                {
                    BeginWatch(t.Key.Name + " Update");
                    t.Value.Update();
                    EndWatch();
                }
                catch (Exception ex)
                {
                    Log.Print(ex);
                }
            }
        }

        void FixedUpdate()
        {
            foreach (var t in modules)
            {
                try
                {
                    BeginWatch(t.Key.Name + " FixedUpdate");
                    t.Value.FixedUpdate();
                    EndWatch();
                }
                catch (Exception ex)
                {
                    Log.Print(ex);
                }
            }
        }

        void LateUpdate()
        {
            foreach (var t in modules)
            {
                try
                {
                    BeginWatch(t.Key.Name + " LateUpdate");
                    t.Value.LateUpdate();
                    EndWatch();
                }
                catch (Exception ex)
                {
                    Log.Print(ex);
                }
            }
        }

        void OnDestroy()
        {
            foreach (var t in modules)
            {
                try
                {
                    t.Value.OnDestroy();
                }
                catch (Exception ex)
                {
                    Log.Print(ex);
                }
            }
        }

        public Dictionary<Type, ModuleBase> modules = new Dictionary<Type, ModuleBase>();

        public void AddPlugin<T>() where T : ModuleBase, new()
        {
            if (!modules.ContainsKey(typeof(T)))
            {
                Log.Print("Run Plugin: " + typeof(T));
                modules.Add(typeof(T), Activator.CreateInstance(typeof(T)) as T);
            }
        }


        public static T GetPlugin<T>() where T : ModuleBase
        {
            if (ins == null)
                ins = GameObject.Find("HACK").GetComponent<Loop>();
            if (ins.modules.TryGetValue(typeof(T), out var m))
                return m as T;
            return null;
        }

        public void InitPlugins()
        {
            AddPlugin<SettingsModule>();
            // AddPlugin<Viewer>();
            AddPlugin<PlayerMgr>();
            AddPlugin<Esp>();
            AddPlugin<Aim>();
            // AddPlugin<Fun>();
            // AddPlugin<Chat>();
            // AddPlugin<Punch>();
            // AddPlugin<Spread>();
            // AddPlugin<Module.Ping>();
        }
    }
}
