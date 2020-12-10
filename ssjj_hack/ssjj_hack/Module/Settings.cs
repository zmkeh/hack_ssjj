using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ssjj_hack.Module
{
    public class Settings : ModuleBase
    {
        public void SetScreen()
        {
            if (Screen.width != 800 || Screen.fullScreen)
            {
                Screen.SetResolution(1280, 720, false);
            }
        }

        public override void Update()
        {
            SetScreen();
        }
    }
}
