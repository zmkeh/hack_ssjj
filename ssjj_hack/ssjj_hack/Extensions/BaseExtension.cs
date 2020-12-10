namespace ssjj_hack
{
    public static class BaseExtension
    {
        public static string Format(this string str, params object[] args)
        {
            return string.Format(str, args);
        }
    }
}
