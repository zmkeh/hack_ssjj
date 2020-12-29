namespace ssjj_hack.Module
{
    public enum AimPos
    {
        HEAD = 1,
        CHEST = 2,
    }

    public class Settings
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

        public static string iniPath => "settings.ini";
        static IniFile ini = new IniFile();

        public static void Read()
        {
            ini.Load(iniPath);

            isEsp = ini["Settings"]["isEsp"].ToBool();
            isEspFriendly = ini["Settings"]["isEspFriendly"].ToBool();
            isEspHp = ini["Settings"]["isEspHp"].ToBool();
            isEspBox = ini["Settings"]["isEspBox"].ToBool();
            isEspAirLine = ini["Settings"]["isEspAirLine"].ToBool();
            isEspAim = ini["Settings"]["isEspAim"].ToBool();

            isAim = ini["Settings"]["isAim"].ToBool();
            aimRadiusPercent = ini[""]["aimRadiusPercent"].ToInt();
            isAimRadius = ini["Settings"]["isAimRadius"].ToBool();
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
            ini["Settings"]["isEspAirLine"] = isEspAirLine;
            ini["Settings"]["isEspAim"] = isEspAim;

            ini["Settings"]["isAim"] = isAim;
            ini["Settings"]["aimRadiusPercent"] = aimRadiusPercent;
            ini["Settings"]["isAimRadius"] = isAimRadius;
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
