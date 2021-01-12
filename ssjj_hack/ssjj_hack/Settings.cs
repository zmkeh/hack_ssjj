using System.IO;
using UnityEngine;

namespace ssjj_hack.Module
{
    public enum AimPos
    {
        HEAD = 0,
        CHEST = 1,
    }

    public class Settings
    {
        public static bool isEsp = true;
        public static bool isEspFriendly = false;
        public static bool isEspHp = true;
        public static bool isEspBox = true;
        public static bool isEspBoneLine = true;
        public static bool isEspAirLine = true;

        public static bool isAim = true;
        public static bool isAimLine = false;
        public static bool isAimCircle = false;
        public static int aimRange = 2;
        public static AimPos aimPos = AimPos.HEAD;

        public static bool isNoRecoil = true;
        public static bool isNoSpread = true;

        public static bool isWindowed = true;
        public static int windowWidth = 1280;
        public static int windowHeight = 720;

        public static string iniPath => Path.Combine(Application.streamingAssetsPath, "settings.ini");
        static IniFile ini = new IniFile();

        public static void Read()
        {
            ini.Load(iniPath);

            isEsp = ini["Settings"]["isEsp"].ToBool();
            isEspFriendly = ini["Settings"]["isEspFriendly"].ToBool();
            isEspHp = ini["Settings"]["isEspHp"].ToBool();
            isEspBox = ini["Settings"]["isEspBox"].ToBool();
            isEspBoneLine = ini["Settings"]["isEspBoneLine"].ToBool();
            isEspAirLine = ini["Settings"]["isEspAirLine"].ToBool();
            isAim = ini["Settings"]["isAim"].ToBool();
            aimRange = ini["Settings"]["aimRange"].ToInt();
            isAimLine = ini["Settings"]["isAimLine"].ToBool();
            isAimCircle = ini["Settings"]["isAimCircle"].ToBool();
            aimPos = (AimPos)ini["Settings"]["aimPos"].ToInt();
            isNoRecoil = ini["Settings"]["isNoRecoil"].ToBool();
            isNoSpread = ini["Settings"]["isNoSpread"].ToBool();
            isWindowed = ini["Settings"]["isWindowed"].ToBool();
            windowWidth = ini["Settings"]["windowWidth"].ToInt();
            windowHeight = ini["Settings"]["windowHeight"].ToInt();
        }
    }
}
