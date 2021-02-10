namespace Lanzou
{
    public class JsonUtility
    {
        public static T FromJson<T>(string json)
        {
            return LitJson.JsonMapper.ToObject<T>(json);
        }
    }
}
