using System;

namespace Lanzou
{
    [Serializable]
    public class GetDirResponse : ResponseBase
    {
        /// <summary>
        /// 
        /// </summary>
        public InfoItem[] info;
        /// <summary>
        /// 子目录集合
        /// </summary>
        public TextItem[] text;

        [Serializable]
        public class InfoItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string name;
            /// <summary>
            /// 
            /// </summary>
            public string folder_des;
            /// <summary>
            /// 
            /// </summary>
            public int folderid;
            /// <summary>
            /// 
            /// </summary>
            public int now;
        }

        [Serializable]
        public class TextItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string onof;
            /// <summary>
            /// 
            /// </summary>
            public string folderlock;
            /// <summary>
            /// 
            /// </summary>
            public string is_lock;
            /// <summary>
            /// 
            /// </summary>
            public string name;
            /// <summary>
            /// 
            /// </summary>
            public string fol_id;
            /// <summary>
            /// 
            /// </summary>
            public string folder_des;
        }
    }
}
