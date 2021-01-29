namespace OpenCredentialPublisher.Services.Extensions
{
    public static class StringExtensions
    {
        public static string EnsureTrailingSlash(this string url)
        {
            return url.EndsWith('/') ? url : string.Concat(url, "/");
        }

        public static string SafeId(this string value)
        {
            return value
                .Replace(":", string.Empty)
                .Replace("/", string.Empty)
                .Replace(".", string.Empty);
        }
    }
}
