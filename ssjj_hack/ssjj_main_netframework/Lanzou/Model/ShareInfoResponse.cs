using System;

namespace Lanzou
{
    [Serializable]
    public class ShareInfoResponse : ResponseBase
    {
        public ShareInfo info;
        public string text;
    }

    [Serializable]
    public class ShareInfo
    {
        public string pwd;
        public string onof;
        public string f_id;
        public string taoc;
        public string is_newd;

        public string url => is_newd + "/" + f_id;
    }
}
