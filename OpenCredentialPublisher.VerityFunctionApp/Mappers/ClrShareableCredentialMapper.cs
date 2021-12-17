using Microsoft.Extensions.Options;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Enums;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Data.ViewModels.Credentials;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Shared.Interfaces;
using OpenCredentialPublisher.Shared.Models;
using OpenCredentialPublisher.Shared.Utilities;
using OpenCredentialPublisher.VerityFunctionApp.Models;
using System;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.VerityFunctionApp.Mappers
{
    public class ClrShareableCredentialMapper : BaseMapper, ICredentialMapper<CredentialMap, ClrShareCredential>
    {
        private readonly LinkService _linkService;
        private readonly CredentialPublisherOptions _credentialPublisherOptions;

        public ClrShareableCredentialMapper(LinkService linkService, IOptions<CredentialPublisherOptions> credentialPublisherOptions)
        {
            _linkService = linkService;
            _credentialPublisherOptions = credentialPublisherOptions?.Value ?? throw new ArgumentNullException($"{CredentialPublisherOptions.Section} is required. Is it missing from the configuration?");
        }
        public async Task<ClrShareCredential> MapAsync(CredentialMap model)
        {
            var clrViewModel = ClrViewModel.FromClrModel(model.Clr);
            var clr = clrViewModel.RawClrDType;

            var additionalProperties = GetAdditionalProperties(clr);

            var link = new LinkModel { ClrForeignKey = model.Clr.ClrId, CredentialRequestId = model.CredentialRequestId, UserId = model.WalletRelationship.UserId, Nickname = $"SentToWallet-{model.WalletRelationship.WalletName}", RequiresAccessKey = true, CreatedAt = DateTime.UtcNow };
            await _linkService.AddAsync(link);

            var shareModel = new ShareModel
            {
                LinkId = link.Id,
                ShareTypeId = ShareTypeEnum.Wallet,
                AccessKey = Crypto.CreateRandomString(16),
                UseCount = 0,
                CreatedAt = DateTime.UtcNow,
                StatusId = StatusEnum.Active
            };

            await _linkService.AddShareAsync(shareModel);

            string url = null;
            if (Uri.TryCreate(_credentialPublisherOptions.HostUrl, UriKind.Absolute, out var baseUri))
            {
                url = LinkService.GetLinkUrl(baseUri, link.Id);
            }

            var credential = new ClrShareCredential
            {
                Clr_Issue_Date = clr.IssuedOn.ToString(),
                Clr_Name = clr.Name,
                Learner_Address = AddressToString(clr.Learner.Address),
                Learner_Name = clr.Learner.Name,
                Learner_StudentId = clr.Learner.StudentId,
                Publisher_Address = AddressToString(clr.Publisher.Address),
                Publisher_Name = clr.Publisher.Name,
                Publisher_ParentOrg = additionalProperties.parentOrg,
                Publisher_Official = additionalProperties.official,
                AccessKey = shareModel.AccessKey,
                Url = url
            };
            return credential;
        }

        
    }
}
