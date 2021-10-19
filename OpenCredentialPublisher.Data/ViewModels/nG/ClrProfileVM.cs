using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Models.ClrEntities;
using System.Collections.Generic;

namespace OpenCredentialPublisher.Data.ViewModels.nG
{
    public class ClrProfileVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string StudentId { get; set; }
        public string SourcedId { get; set; }
        public string Url { get; set; }
        public AddressDType Address { get; set; }
        public Dictionary<string, object> AdditionalProperties { get; set; }

        public static ClrProfileVM FromClrProfile(ProfileModel profile)
        {
            if (profile == null)
            {
                return new ClrProfileVM
                {
                    Address = new AddressDType(),
                    AdditionalProperties = new Dictionary<string, object>()
                };
            }
            else
            {
                return new ClrProfileVM
                {
                    Address = profile.Address,
                    AdditionalProperties = profile.AdditionalProperties,
                    Email = profile.Email,
                    Telephone = profile.Telephone,
                    Id = profile.Id,
                    Name = profile.Name,
                    SourcedId = profile.SourcedId,
                    StudentId = profile.StudentId,
                    Url = profile.Url
                };
            }
        }

    }
}