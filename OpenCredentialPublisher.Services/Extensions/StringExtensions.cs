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

        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;
            return char.ToLowerInvariant(value[0]) + (value.Length > 1 ? value.Substring(1) : string.Empty);
        }
    }
}
