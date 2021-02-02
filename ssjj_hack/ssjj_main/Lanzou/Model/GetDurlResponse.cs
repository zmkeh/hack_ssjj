using System;

namespace Lanzou
{
    [Serializable]
    public class GetDurlResponse : ResponseBase
    {
        public string dom;
        public string url;
    }
}
