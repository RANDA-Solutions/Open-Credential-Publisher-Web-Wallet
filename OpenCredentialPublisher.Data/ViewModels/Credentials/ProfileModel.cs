using OpenCredentialPublisher.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Data.ViewModels.Credentials
{
    public class UserProfileModel
    {
        public bool HasProfileImage { get; set; }
        public string ProfileImageUrl { get; set; }
        public string DisplayName { get; set; }
        public bool MissingDisplayName => String.IsNullOrEmpty(DisplayName);
        public Int32 Credentials { get; set; }
        public Int32 Achievements { get; set; }
        public Int32 Scores { get; set; }
        public Int32 ActiveLinks { get; set; }
        public Dictionary<string, string> AdditionalData { get; set; }
    }
}
