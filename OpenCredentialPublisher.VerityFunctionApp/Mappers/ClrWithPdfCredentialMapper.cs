using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using OpenCredentialPublisher.ClrLibrary.Extensions;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Extensions;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Enums;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Data.ViewModels.Credentials;
using OpenCredentialPublisher.Services.Drawing;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Shared.Interfaces;
using OpenCredentialPublisher.Shared.Models;
using OpenCredentialPublisher.Shared.Utilities;
using OpenCredentialPublisher.VerityFunctionApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.VerityFunctionApp.Mappers
{
    public class ClrWithPdfCredentialMapper : BaseMapper, ICredentialMapper<CredentialMap, ClrWithPdfCredential>
    {
        private readonly LinkService _linkService;
        private readonly CredentialService _credentialService;
        private readonly CredentialPublisherOptions _credentialPublisherOptions;
        public ClrWithPdfCredentialMapper(CredentialService credentialService, LinkService linkService, IOptions<CredentialPublisherOptions> credentialPublisherOptions)
        {
            _credentialService = credentialService;
            _linkService = linkService;
            _credentialPublisherOptions = credentialPublisherOptions?.Value ?? throw new ArgumentNullException($"{CredentialPublisherOptions.Section} is required. Is it missing from the configuration?");
        }

        public async Task<ClrWithPdfCredential> MapAsync(CredentialMap model)
        {
            var clrViewModel = ClrViewModel.FromClrModel(model.Clr);
            var clr = clrViewModel.RawClrDType;

            var artifact = await _credentialService.CredentialPackagePdfArtifactAsync(model.Clr.CredentialPackageId);
            var pdfDataUrlParts = DataUrlUtility.ParseDataUrl(artifact.Url);

            var link = new LinkModel { ClrForeignKey = model.Clr.ClrId, CredentialRequestId = model.CredentialRequestId, UserId = model.WalletRelationship.UserId, Nickname = $"SentToWallet-{model.WalletRelationship.WalletName}", RequiresAccessKey = true, CreatedAt = DateTimeOffset.UtcNow };
            await _linkService.AddAsync(link);

            var shareModel = new ShareModel
            {
                LinkId = link.Id,
                ShareTypeId = ShareTypeEnum.Wallet,
                AccessKey = Crypto.CreateRandomString(16),
                UseCount = 0,
                CreatedOn = DateTimeOffset.UtcNow,
                StatusId = StatusEnum.Active
            };

            await _linkService.AddShareAsync(shareModel);
            string url = null;
            if (Uri.TryCreate($"{_credentialPublisherOptions.HostUrl}/Links/Display/{link.Id}", UriKind.Absolute, out var uri))
            {
                url = uri.ToString();
            }

            var pdfBytes = PdfUtility.AppendQRCodePage(pdfDataUrlParts.bytes, url, shareModel.AccessKey);
            var additionalProperties = GetAdditionalProperties(clr);

            var credential = new ClrWithPdfCredential
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
                TranscriptLink = new LinkData
                {
                    MimeType = "application/pdf",
                    Extension = "pdf",
                    Name = $"{artifact.Name}-{Guid.NewGuid().ToString().Replace("-", "")}.pdf",
                    Data = new Base64Data
                    {
                        Base64 = Convert.ToBase64String(pdfBytes)
                    }
                }
                //,
                //Clr = new AttachmentData
                //{
                //    MimeType = "application/json",
                //    Filename = "clr-transcript.json",
                //    Data = new Base64Data
                //    {
                //        Base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(clrViewModel.RawClrDType.ToJson()))
                //    }
                //}
            };
            return credential;
        }
    }
}
