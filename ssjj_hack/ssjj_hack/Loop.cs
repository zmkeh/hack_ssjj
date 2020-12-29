using ssjj_hack.Module;
using System;
using System.Collections.Generic;
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
                    t.Value.OnGUI();
                }
                catch (Exception ex)
                {
                    Log.Print(ex);
                }
            }
        }

        void Update()
        {
            foreach (var t in modules)
            {
                try
                {
                    t.Value.Update();
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
                    t.Value.FixedUpdate();
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
                    t.Value.LateUpdate();
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
            AddPlugin<Settings>();
            AddPlugin<Viewer>();
            AddPlugin<PlayerMgr>();
            AddPlugin<Esp>();
            AddPlugin<Aim>();
        }
    }
}
