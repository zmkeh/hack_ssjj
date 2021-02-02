using System;

namespace Lanzou
{
    [Serializable]
    public class GetFilesResponse : ResponseBase
    {
        /// <summary>
        /// 
        /// </summary>
        public int info;
        /// <summary>
        /// 
        /// </summary>
        public TextItem[] text;

        [Serializable]
        public class TextItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string icon;
            /// <summary>
            /// 
            /// </summary>
            public string id;
            /// <summary>
            /// 
            /// </summary>
            public string name_all;
            /// <summary>
            /// 
            /// </summary>
            public string name;
            /// <summary>
            /// 
            /// </summary>
            public string size;
            /// <summary>
            /// 21 分钟前
            /// </summary>
            public string time;
            /// <summary>
            /// 
            /// </summary>
            public string downs;
            /// <summary>
            /// 
            /// </summary>
            public string onof;
            /// <summary>
            /// 
            /// </summary>
            public string is_lock;
            /// <summary>
            /// 
            /// </summary>
            public string filelock;
            /// <summary>
            /// 
            /// </summary>
            public int is_des;
            /// <summary>
            /// 
            /// </summary>
            public int is_ico;
        }
    }
}
