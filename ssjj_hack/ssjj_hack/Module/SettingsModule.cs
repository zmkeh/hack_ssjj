using System.IO;
using UnityEngine;

namespace ssjj_hack.Module
{
    public class SettingsModule : ModuleBase
    {
        public void SetScreen()
        {
            if (!Settings.isWindowed)
            {
                if (!Screen.fullScreen)
                {
                    Screen.fullScreen = true;
                }
            }
            else
            {
                if (Screen.width != Settings.windowWidth
                  || Screen.height != Settings.windowHeight
                  || Screen.fullScreen)
                {
                    Screen.SetResolution(Settings.windowWidth, Settings.windowHeight, false);
                }
            }
        }

        private long lastIniTicks = 0;

        public override void Update()
        {
            SetScreen();
        }

        private long GetTicks()
        {
            return new FileInfo(Settings.iniPath).LastWriteTime.Ticks;
        }

        public override void Start()
        {
            base.Start();
            lastIniTicks = GetTicks();
            Settings.Read();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (lastIniTicks != GetTicks())
            {
                Log.Print("Ini Changed: ");
                lastIniTicks = GetTicks();
                Settings.Read();
            }
        }
    }
}
