using Entitas;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ssjj_hack
{
    public class Fun : ModuleBase
    {
        public override void OnGUI()
        {
            if (GUILayout.Button("Test"))
            {
                Log.Print(DateTime.Now.ToString() + DateTime.Now.ToString() + DateTime.Now.ToString());
            }
        }
    }

}
