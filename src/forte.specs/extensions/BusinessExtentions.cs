namespace forte.extensions
{
    public static class BusinessExtentions
    {
        public static string GetAppleTvLink(this string link)
        {
            return $"{link}(format=m3u8-aapl,audio-only=false)";
        }
    }
}
