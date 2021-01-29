// ReSharper disable UnusedMember.Global
namespace OpenCredentialPublisher.ClrLibrary
{
    public static class ClrConstants
    {
        public static class JsonLd
        {
            public static string Context = "https://purl.imsglobal.org/spec/clr/v1p0/context/clr_v1p0.jsonld";
        }

        public static class MediaTypes
        {
            public static string JsonMediaType = "application/json";
            public static string JsonLdMediaType = "application/ld+json";
        }

        public static class Scopes
        {
            public static string Delete = "https://purl.imsglobal.org/spec/clr/v1p0/scope/delete";
            public static string Readonly = "https://purl.imsglobal.org/spec/clr/v1p0/scope/readonly";
            public static string Replace = "https://purl.imsglobal.org/spec/clr/v1p0/scope/replace";

            public static string[] AllScopes = 
            {
                Delete,
                Readonly,
                Replace
            };
        }
    }
}
