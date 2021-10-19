using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Models.ClrEntities;
using System;
using System.Collections.Generic;

namespace OpenCredentialPublisher.Data.ViewModels.nG.ClrSimplified
{
    public class ProfileVM
    {
        public int ProfileId { get; set; }
        public bool IsEndorsementProfile { get; set; }
        //IBaseEntity properties
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public string AdditionalName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
        public List<SystemIdentifierDType> Identifiers { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Official { get; set; }
        public virtual CryptographicKeyDType PublicKey { get; set; }
        public string RevocationList { get; set; }
        public string SourcedId { get; set; }
        public string StudentId { get; set; }
        public string Telephone { get; set; }
        public string Url { get; set; }
        public AddressDType Address { get; set; }
        public VerificationVM Verification { get; set; }


        public static ProfileVM FromModel(ProfileModel profile)
        {
            if (profile == null)
            {
                return null;
            }

            return new ProfileVM
            {
                AdditionalName = profile.AdditionalName,
                BirthDate = profile.BirthDate,
                CreatedAt = profile.CreatedAt,
                Description = profile.Description,
                Email = profile.Email,
                FamilyName = profile.FamilyName,
                GivenName = profile.GivenName,
                Id = profile.Id,
                Identifiers = profile.Identifiers,
                Image = profile.Image,
                IsDeleted = profile.IsDeleted,
                IsEndorsementProfile = profile.IsEndorsementProfile,
                ModifiedAt = profile.ModifiedAt,
                Name = profile.Name,
                Official = profile.Official,
                ProfileId = profile.ProfileId,
                PublicKey = profile.PublicKey,
                RevocationList = profile.RevocationList,
                SourcedId = profile.SourcedId,
                StudentId = profile.StudentId,
                Telephone = profile.Telephone,
                Type = profile.Type
            };
        }
    }
}
