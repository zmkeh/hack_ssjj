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

        public static bool isWindowed = false;
        public static int windowWidth = 1280;
        public static int windowHeight = 720;

        public static string root => "ssjj_libs";
        public static string iniPath => $"{root}/settings.ini";
        static IniFile ini = new IniFile();

        public static void Read()
        {
            ini.Load(iniPath);

            isEsp = ini["Settings"]["isEsp"].ToBool();
            isEspFriendly = ini["Settings"]["isEspFriendly"].ToBool();
            isEspHp = ini["Settings"]["isEspHp"].ToBool();
            isEspBoneLine = ini["Settings"]["isEspBoneLine"].ToBool();
            isEspBox = ini["Settings"]["isEspBox"].ToBool();
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


        public static void Save()
        {
            ini["Settings"]["isEsp"] = isEsp;
            ini["Settings"]["isEspFriendly"] = isEspFriendly;
            ini["Settings"]["isEspHp"] = isEspHp;
            ini["Settings"]["isEspBox"] = isEspBox;
            ini["Settings"]["isEspBoneLine"] = isEspBoneLine;
            ini["Settings"]["isEspAirLine"] = isEspAirLine;

            ini["Settings"]["isAim"] = isAim;
            ini["Settings"]["aimRange"] = aimRange;
            ini["Settings"]["isAimLine"] = isAimLine;
            ini["Settings"]["isAimCircle"] = isAimCircle;
            ini["Settings"]["aimPos"] = (int)aimPos;

            ini["Settings"]["isNoRecoil"] = isNoRecoil;
            ini["Settings"]["isNoSpread"] = isNoSpread;

            ini["Settings"]["isWindowed"] = isWindowed;
            ini["Settings"]["windowWidth"] = windowWidth;
            ini["Settings"]["windowHeight"] = windowHeight;

            ini.Save(iniPath);
        }
    }
}
