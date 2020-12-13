using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ssjj_hack.Module
{
    public class Settings : ModuleBase
    {
        public int screenWidth = 1280;
        public int screenHeight = 720;

        public void SetScreen()
        {
            if (Screen.width != screenWidth || Screen.fullScreen)
            {
                Screen.SetResolution(screenWidth, screenHeight, false);
            }
        }

        public override void Update()
        {
            SetScreen();
        }
    }
}
