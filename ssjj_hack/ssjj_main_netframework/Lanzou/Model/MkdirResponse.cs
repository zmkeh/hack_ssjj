using System;

namespace Lanzou
{
    [Serializable]
    public class MkdirResponse : ResponseBase
    {
        /// <summary>
        /// 创建成功
        /// </summary>
        public string info;
        /// <summary>
        /// ID
        /// </summary>
        public string text;
    }
}
