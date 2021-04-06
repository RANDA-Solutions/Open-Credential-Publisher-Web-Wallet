namespace OpenCredentialPublisher.ObcLibrary
{
    public static class ObcConstants
    {
        public static class MediaTypes
        {
            public static string JsonMediaType = "application/json";
            public static string JsonLdMediaType = "application/ld+json";
        }

        public static class Scopes
        {
            public static string AssertionReadonly = "https://purl.imsglobal.org/spec/ob/v2p1/scope/assertion.readonly";
            public static string AssertionCreate = "https://purl.imsglobal.org/spec/ob/v2p1/scope/assertion.create";
            public static string ProfileReadonly = "https://purl.imsglobal.org/spec/ob/v2p1/scope/profile.readonly";
            public static string ProfileUpdate = "https://purl.imsglobal.org/spec/ob/v2p1/scope/profile.update";

            public static string[] AllScopes = 
            {
                AssertionReadonly,
                AssertionCreate,
                ProfileReadonly,
                ProfileUpdate
            };
        }
    }
}
