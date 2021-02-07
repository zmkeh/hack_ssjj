using System;
using System.Reflection;
using UnityEngine;

namespace ssjj_hack
{
    public class Fun : ModuleBase
    {
        public override void FixedUpdate()
        {
            if (!Application.runInBackground)
            {
                Application.runInBackground = true;
            }
        }
    }
}
