using System;

namespace Lanzou
{

    [Serializable]
    public class ResponseBase
    {
        public int zt;
    }

    [Serializable]
    public class LanZouFileResult : ResponseBase
    {
        public string info;
        public LanZouFileResultInfo[] text;

    }

    [Serializable]
    public class LanZouFileResultInfo
    {
        public string icon;
        public string id;
        public string f_id;
        public string name_all;
        public string name;
        public string size;
        public string time;
        public string downs;
        public string onof;
        public string is_newd;

    }

    [Serializable]
    public class LanZouResult : ResponseBase
    {
        public LanZouInfo info;
        public string text;

    }

    [Serializable]
    public class LanZouInfo
    {
        public string pwd;
        public string onof;
        public string f_id;
        public string taoc;
        public string is_newd;
    }

    [Serializable]
    public class LanZouPwd : ResponseBase
    {
        public string info;
        public string text;
    }
}
