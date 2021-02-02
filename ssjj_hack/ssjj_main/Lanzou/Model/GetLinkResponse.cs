using System;

namespace Lanzou
{
    [Serializable]
    public class GetLinkResponse : ResponseBase
    {
        public string dom;

        public int inf;

        public string url;

        /// <summary>
        /// 完整地址
        /// </summary>
        public string FullUrl => dom + "/file/" + url;
    }
}
