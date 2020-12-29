﻿using System.IO;
using UnityEngine;

namespace ssjj_hack.Module
{

    public enum AimPos
    {
        HEAD = 1,
        CHEST = 2,
    }

    public class Settings : ModuleBase
    {
        public static bool isEsp;
        public static bool isEspFriendly;
        public static bool isEspHp;
        public static bool isEspBox;
        public static bool isEspAirLine;
        public static bool isEspAim;

        public static bool isAim;
        public static int aimRadiusPercent;
        public static bool isAimRadius;
        public static AimPos aimPos;

        public static bool isNoRecoil;
        public static bool isNoSpread;

        public static bool isWindowed = false;
        public static int windowWidth = 1280;
        public static int windowHeight = 720;

        private string iniPath => Path.Combine(Application.streamingAssetsPath, "settings.ini");
        IniFile ini = new IniFile();
        FileSystemWatcher watcher = new FileSystemWatcher();

        public void SetScreen()
        {
            if (!isWindowed)
            {
                if (!Screen.fullScreen)
                {
                    Screen.fullScreen = true;
                }
            }
            else
            {
                if (Screen.width != windowWidth
                  || Screen.height != windowHeight
                  || Screen.fullScreen)
                {
                    Screen.SetResolution(windowWidth, windowHeight, false);
                }
            }
        }

        public override void Update()
        {
            SetScreen();
        }

        public override void Start()
        {
            base.Start();

            watcher.BeginInit();
            watcher.Path = iniPath;
            watcher.Changed += Watcher_Changed;
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            Log.Print("Ini Changed: " + e.ChangeType);

            ini.Load(iniPath);

            isEsp = ini["Settings"]["isEsp"].ToBool();
            isEspFriendly = ini["Settings"]["isEspFriendly"].ToBool();
            isEspHp = ini["Settings"]["isEspHp"].ToBool();
            isEspBox = ini["Settings"]["isEspBox"].ToBool();
            isEspAirLine = ini["Settings"]["isEspAirLine"].ToBool();
            isEspAim = ini["Settings"]["isEspAim"].ToBool();

            isAim = ini["Settings"]["isAim"].ToBool();
            aimRadiusPercent = ini["Settings"]["aimRadiusPercent"].ToInt();
            isAimRadius = ini["Settings"]["isAimRadius"].ToBool();
            aimPos = (AimPos)ini["Settings"]["aimPos"].ToInt();

            isNoRecoil = ini["Settings"]["isNoRecoil"].ToBool();
            isNoSpread = ini["Settings"]["isNoSpread"].ToBool();

            isWindowed = ini["Settings"]["isWindowed"].ToBool();
            windowWidth = ini["Settings"]["windowWidth"].ToInt();
            windowHeight = ini["Settings"]["windowHeight"].ToInt();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

        }
    }
}
