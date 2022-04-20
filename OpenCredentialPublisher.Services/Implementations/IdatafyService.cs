using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models.Idatafy;
using OpenCredentialPublisher.Data.Options;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class IdatafyService
    {
        private readonly CredentialPackageService _credentialPackageService;
        private readonly CredentialService _credentialService;
        private readonly WalletDbContext _context;
        private readonly IdatafyOptions _idatafyOptions;
        private readonly ILogger<IdatafyService> _logger;
        public IdatafyService(CredentialPackageService credentialPackageService
            , CredentialService credentialService
            , WalletDbContext context
            , IOptions<IdatafyOptions> idatafyOptions
            , ILogger<IdatafyService> logger)
        {
            _credentialPackageService = credentialPackageService;
            _credentialService = credentialService;
            _context = context;
            _idatafyOptions = idatafyOptions?.Value ?? throw new Exception("You must specify IdatafyOptions if loading this service.");
            _logger = logger;
        }


        public async Task<string> SendSmartResumeAsync(string userId, int packageId, int clrId)
        {
            try
            {
                var clr = await _credentialService.GetClrAsync(userId, clrId);
                if (clr == null)
                {
                    throw new Exception($"The CLR with id {clrId} could not be found.");
                }

                var clrDType = JsonConvert.DeserializeObject<ClrDType>(clr.Json);
                if (_idatafyOptions.UseUserEmail)
                {
                    var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
                    clrDType.Learner.Email = user.Email;
                }
                var jsonFile = clrDType.ToJson();

                var sftp = new SftpClient(_idatafyOptions.Server
                    , _idatafyOptions.Port
                    , _idatafyOptions.Username
                    , _idatafyOptions.Password);

                sftp.Connect();

                sftp.ChangeDirectory(_idatafyOptions.DropFolder);
                var uploadName = $"{userId}-{clrId}-{DateTime.UtcNow.Ticks}.json";
                using (var stream = new MemoryStream(UTF8Encoding.UTF8.GetBytes(jsonFile)))
                {
                    sftp.UploadFile(stream, $"{userId}-{clrId}-{DateTime.UtcNow.Ticks}.json");
                }

                var smartResume = new SmartResume
                {
                    UserId = userId,
                    ClrId = clrId,
                    UploadName = uploadName,
                    IsReady = false,
                    CreatedAt = DateTime.UtcNow,
                    SmartResumeUrl = _idatafyOptions.SmartResumeUrl
                };

                clr.SmartResume = smartResume;

                await _context.SmartResumes.AddAsync(smartResume);
                await _context.SaveChangesAsync();

                return _idatafyOptions.SmartResumeUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, userId, packageId, clrId);
                throw;
            }

        }

        public async Task UpdateSmartResumeAsync(string uploadName)
        {
            var smartResume = await _context.SmartResumes.FirstOrDefaultAsync(u => u.UploadName == uploadName);
            smartResume.IsReady = true;
            smartResume.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

    }
}
