using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Dtos.Account;
using OpenCredentialPublisher.Data.Dtos.ProofRequest;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.MSProofs;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Data.ViewModels.nG;
using OpenCredentialPublisher.Services.Drawing;
using OpenCredentialPublisher.Services.Implementations;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Controllers.Account
{
    [ApiController]
    public class AzADProofController : ApiController<AzADProofController>
    {
        const string PRESENTATIONPAYLOAD = "presentation_request_config.json";
        const string ISSUANCEPAYLOAD = "issuance_request_config.json";

        private const string StateCookieName = "AzADLoginProofState";
        private readonly AzLoginProofService _loginProofService;
        private readonly EmailVerificationService _emailVerificationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly LogHttpClientService _logHttpClientService;
        protected readonly MSVCOptions _msvcOptions;
        private readonly WalletDbContext _context;
        private string _apiKey;

        protected IMemoryCache _cache;

        public AzADProofController(
            WalletDbContext context,
            AzLoginProofService loginProofService,
            EmailVerificationService emailVerificationService,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IMemoryCache memoryCache,
            LogHttpClientService logHttpClientService,
            IOptions<MSVCOptions> msvcOptions,
            ILogger<AzADProofController> logger) : base(logger)
        {
            _context = context;
            _loginProofService = loginProofService;
            _emailVerificationService = emailVerificationService;
            _signInManager = signInManager;
            _userManager = userManager;
            _cache = memoryCache;
            _logHttpClientService = logHttpClientService;
            _msvcOptions = msvcOptions.Value;
            _apiKey = System.Environment.GetEnvironmentVariable("API-KEY");
        }
                
        [HttpGet]
        [Route("GetProof")]
        public async Task<ActionResult> GetProofAsync()
        {
            try
            {

                string jsonString = null;
                //they payload template is loaded from disk and modified in the code below to make it easier to get started
                //and having all config in a central location appsettings.json. 
                //if you want to manually change the payload in the json file make sure you comment out the code below which will modify it automatically
                //
                string payloadpath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), PRESENTATIONPAYLOAD);
                _logger.LogTrace("IssuanceRequest file: {0}", payloadpath);
                if (!System.IO.File.Exists(payloadpath))
                {
                    _logger.LogError("File not found: {0}", payloadpath);
                    ModelState.AddModelError("", $"{PRESENTATIONPAYLOAD} not found");
                    return ApiModelInvalid(ModelState);
                }
                jsonString = System.IO.File.ReadAllText(payloadpath);
                if (string.IsNullOrEmpty(jsonString))
                {
                    _logger.LogError("Error reading file: {0}", payloadpath);
                    ModelState.AddModelError("", $"{PRESENTATIONPAYLOAD} error reading file");
                    return ApiModelInvalid(ModelState);
                }

                string state = Guid.NewGuid().ToString();

                //modify payload with new state, the state is used to be able to update the UI when callbacks are received from the VC Service
                JObject payload = JObject.Parse(jsonString);
                if (payload["callback"]["state"] != null)
                {
                    payload["callback"]["state"] = state;
                }

                //get the VerifierDID from the appsettings
                if (payload["authority"] != null)
                {
                    payload["authority"] = _msvcOptions.VerifierAuthority;
                }

                //copy the issuerDID from the settings and fill in the trustedIssuer part of the payload
                //this means only that issuer should be trusted for the requested credentialtype
                //this value is an array in the payload, you can trust multiple issuers for the same credentialtype
                //very common to accept the test VCs and the Production VCs coming from different verifiable credential services
                if (payload["presentation"]["requestedCredentials"][0]["acceptedIssuers"][0] != null)
                {
                    payload["presentation"]["requestedCredentials"][0]["acceptedIssuers"][0] = _msvcOptions.IssuerAuthority;
                }

                //modify the callback method to make it easier to debug with tools like ngrok since the URI changes all the time
                //this way you don't need to modify the callback URL in the payload every time ngrok changes the URI
                if (payload["callback"]["url"] != null)
                {
                    //localhost hostname can't work for callbacks so we will use the configured value in appsetttings.json in that case.
                    //this happens for example when testing with sign-in to an IDP and https://localhost is used as redirect URI
                    string host = GetRequestHostName();
                    if (!host.Contains("//localhost"))
                    {
                        payload["callback"]["url"] = String.Format("{0}/api/AzADProof/presentationCallback", host);
                    }
                    else
                    {
                        if (!String.IsNullOrWhiteSpace(_msvcOptions.VCCallbackHostURL))
                        {
                            payload["callback"]["url"] = String.Format("{0}/api/AzADProof/presentationCallback", _msvcOptions.VCCallbackHostURL);
                        }
                        else
                        {
                            _logger.LogError(String.Format("VCCallbackHostURL is not set in the AppSettings section of appsettings.json file. Please refer to README section on Running the sample for instructions on how to set this value."));
                        }
                    }
                }

                // set our api-key in the request so we can check it in the callbacks we receive
                if (payload["callback"]["headers"]["api-key"] != null)
                {
                    payload["callback"]["headers"]["api-key"] = this._apiKey;
                }

                jsonString = JsonConvert.SerializeObject(payload);


                //CALL REST API WITH PAYLOAD
                HttpStatusCode statusCode = HttpStatusCode.OK;
                string response = null;
                try
                {
                    //The VC Request API is an authenticated API. We need to clientid and secret (or certificate) to create an access token which 
                    //needs to be send as bearer to the VC Request API
                    var accessToken = await GetAccessToken();
                    if (accessToken.Item1 == String.Empty)
                    {
                        _logger.LogError(String.Format("failed to acquire accesstoken: {0} : {1}"), accessToken.error, accessToken.error_description);
                        ModelState.AddModelError(accessToken.error, accessToken.error_description);
                        return ApiModelInvalid(ModelState);
                    }

                    HttpClient client = new HttpClient();
                    var defaultRequestHeaders = client.DefaultRequestHeaders;
                    defaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.token);

                    HttpResponseMessage res = await client.PostAsync(_msvcOptions.ApiEndpoint, new StringContent(jsonString, Encoding.UTF8, "application/json"));
                    response = await res.Content.ReadAsStringAsync();
                    client.Dispose();
                    statusCode = res.StatusCode;

                    if (statusCode == HttpStatusCode.Created)
                    {
                        _logger.LogTrace("succesfully called Request API");
                        JObject requestConfig = JObject.Parse(response);
                        requestConfig.Add(new JProperty("id", state));
                        jsonString = JsonConvert.SerializeObject(requestConfig);

                        //We use in memory cache to keep state about the request. The UI will check the state when calling the presentationResponse method

                        var cacheData = new
                        {
                            status = "notscanned",
                            message = "Request ready, please scan with Authenticator",
                            expiry = requestConfig["expiry"].ToString()
                        };
                        _cache.Set(state, JsonConvert.SerializeObject(cacheData));

                        var qrCodeBytes = QRCodeUtility.Create(requestConfig["url"].ToString());

                        var qrCode = Convert.ToBase64String(qrCodeBytes, 0, qrCodeBytes.Length);

                        var msLoginProofRequest = new AzLoginProofGetResponseModel(requestConfig["requestId"].ToString(), requestConfig["url"].ToString(), requestConfig["expiry"].ToString(), state, qrCode);

                        _context.MSLoginProofRequests.Add(msLoginProofRequest);
                        await _context.SaveChangesAsync();

                        return ApiOk(msLoginProofRequest);
                    }
                    else
                    {
                        _logger.LogError("Unsuccesfully called Request API: " + response);
                        ModelState.AddModelError("", $"Something went wrong calling the API: {response}" );
                        return ApiModelInvalid(ModelState);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Something went wrong calling the API: {ex.Message}");
                    return ApiModelInvalid(ModelState);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return ApiModelInvalid(ModelState);
            }
        }
        /// <summary>
        /// This method is called by the VC Request API when the user scans a QR code and presents a Verifiable Credential to the service
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("PresentationCallback")]
        public async Task<ActionResult> PresentationCallback()
        {
            try
            {
                var message = string.Empty;
                var status = new AzLoginProofStatusModel();
                string content = await new System.IO.StreamReader(this.Request.Body).ReadToEndAsync();
                await _logHttpClientService.LogRequestAsync(this.Request, content);
                Debug.WriteLine("callback!: " + content);
                JObject presentationResponse = JObject.Parse(content);
                var state = presentationResponse["state"].ToString();
                var requestId = presentationResponse["requestId"].ToString();

                //there are 2 different callbacks. 1 if the QR code is scanned (or deeplink has been followed)
                //Scanning the QR code makes Authenticator download the specific request from the server
                //the request will be deleted from the server immediately.
                //That's why it is so important to capture this callback and relay this to the UI so the UI can hide
                //the QR code to prevent the user from scanning it twice (resulting in an error since the request is already deleted)
                if (presentationResponse["code"].ToString() == "request_retrieved")
                {
                    
                    var cacheData = new
                    {
                        status = "request_retrieved",
                        message = message,
                    };
                    _cache.Set(state, JsonConvert.SerializeObject(cacheData));
                    status = new AzLoginProofStatusModel(requestId, presentationResponse["code"].ToString(), state, content);
                    _context.MSLoginProofStatuses.Add(status);
                    await _context.SaveChangesAsync();
                }

                // the 2nd callback is the result with the verified credential being verified.
                // typically here is where the business logic is written to determine what to do with the result
                // the response in this callback contains the claims from the Verifiable Credential(s) being presented by the user
                // In this case the result is put in the in memory cache which is used by the UI when polling for the state so the UI can be updated.
                if (presentationResponse["code"].ToString() == "presentation_verified")
                {
                    
                    var cacheData = new
                    {
                        status = "presentation_verified",
                        message = message,
                        payload = presentationResponse["issuers"].ToString(),
                        subject = presentationResponse["subject"].ToString(),
                        email = presentationResponse["issuers"][0]["claims"]["email"].ToString(),
                        //firstName = presentationResponse["issuers"][0]["claims"]["firstName"].ToString(),
                        //lastName = presentationResponse["issuers"][0]["claims"]["lastName"].ToString()

                    };
                    _cache.Set(state, JsonConvert.SerializeObject(cacheData));
                    status = new AzLoginProofStatusModel(requestId, presentationResponse["code"].ToString(), state, content);
                    _context.MSLoginProofStatuses.Add(status);
                    await _context.SaveChangesAsync();
                }

                return ApiOk(new MSProofStatus { Message = message});
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return ApiModelInvalid(ModelState);
            }
        }
        [HttpGet]
        [Route("ProofStatus/{requestId}")]
        public async Task<IActionResult> GetProofStatusAsync(string requestId)
        {
            var result = new MSProofStatus();
            var message = string.Empty;
            var statusCode = string.Empty;
            try
            {
                var status = await _loginProofService.GetLoginProofStatusAsync(requestId);

                if (status.Code == "presentation_verified")
                {
                    message = "Presentation verified";
                    statusCode = "Accepted";
                    JObject presentationResponse = JObject.Parse(status.Json);
                    var state = presentationResponse["state"].ToString();
                    var email = presentationResponse["issuers"][0]["claims"]["email"].ToString();
                    var user = await _userManager.FindByEmailAsync(email);
                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = email,
                            Email = email,
                            EmailConfirmed = true,
                            NormalizedEmail = email.ToLower(),
                            NormalizedUserName = email.ToLower()
                        };

                        var identityResult = await _userManager.CreateAsync(user);
                        if (!identityResult.Succeeded)
                        {
                            ModelState.AddModelError("", "There was a problem signing you in.  Please try again later.");
                        }
                        result.NewAccount = true;
                    }

                    if (ModelState.IsValid)
                    {
                        await _signInManager.SignInAsync(user, false);
                        //await _loginProofService.SetLoginProofStatusAsync(id, Data.Models.Enums.StatusEnum.Used);
                    }
                    else
                    {
                        return ApiModelInvalid(ModelState);
                    }
                }
                else if (status.Code == "request_retrieved")
                {
                    message = "QR Code is scanned. Waiting for validation...";
                }
                else if (status.Code == "no_status_yet")
                {
                    message = "Waiting for Proof response...";
                    statusCode = "Waiting";
                }
                else
                {
                    message = status.Code;
                    statusCode = "Unexpected";
                }
                result.Message = message;
                result.Status = statusCode;
                return ApiOk(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
        //**********************************************************************************************
        // Methods for Testing Environments only
        //**********************************************************************************************

        /// <summary>
        /// This method is called from the UI to initiate the issuance of the verifiable credential
        /// </summary>
        /// <returns>JSON object with the address to the presentation request and optionally a QR code and a state value which can be used to check on the response status</returns>
        [HttpPost]
        [Route("VerifyEmail/{key}")]
        public async Task<ActionResult> VerifyEmail(string key)
        {
            try
            {
                var emailVerification = await _emailVerificationService.GetEmailVerificationByKeyAsync(key);
                var email = emailVerification.EmailAddress;
                //they payload template is loaded from disk and modified in the code below to make it easier to get started
                //and having all config in a central location appsettings.json. 
                //if you want to manually change the payload in the json file make sure you comment out the code below which will modify it automatically
                //
                string jsonString = null;
                string newpin = null;

                string payloadpath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), ISSUANCEPAYLOAD);
                _logger.LogTrace("IssuanceRequest file: {0}", payloadpath);
                if (!System.IO.File.Exists(payloadpath))
                {
                    _logger.LogError("File not found: {0}", payloadpath);

                    ModelState.AddModelError("", $"{ISSUANCEPAYLOAD} not found");
                    return ApiModelInvalid(ModelState);
                }
                jsonString = System.IO.File.ReadAllText(payloadpath);
                if (string.IsNullOrEmpty(jsonString))
                {
                    _logger.LogError("Error reading file: {0}", payloadpath);
                    ModelState.AddModelError("", $"{ISSUANCEPAYLOAD} error reading file");
                    return ApiModelInvalid(ModelState);
                }

                //check if pin is required, if found make sure we set a new random pin
                //pincode is only used when the payload contains claim value pairs which results in an IDTokenhint
                JObject payload = JObject.Parse(jsonString);
                if (payload["issuance"]["pin"] != null)
                {
                    if (IsMobile())
                    {
                        _logger.LogTrace("pin element found in JSON payload, but on mobile so remove pin since we will be using deeplinking");
                        //consider providing the PIN through other means to your user instead of removing it.
                        payload["issuance"]["pin"].Parent.Remove();

                    }
                    else
                    {
                        _logger.LogTrace("pin element found in JSON payload, modifying to a random number of the specific length");
                        var length = (int)payload["issuance"]["pin"]["length"];
                        var pinMaxValue = (int)Math.Pow(10, length) - 1;
                        var randomNumber = RandomNumberGenerator.GetInt32(1, pinMaxValue);
                        newpin = string.Format("{0:D" + length.ToString() + "}", randomNumber);
                        payload["issuance"]["pin"]["value"] = newpin;
                    }

                }
                string state = Guid.NewGuid().ToString();

                //modify payload with new state, the state is used to be able to update the UI when callbacks are received from the VC Service
                if (payload["callback"]["state"] != null)
                {
                    payload["callback"]["state"] = state;
                }

                //get the IssuerDID from the appsettings
                if (payload["authority"] != null)
                {
                    payload["authority"] = _msvcOptions.IssuerAuthority;
                }

                //modify the callback method to make it easier to debug 
                //with tools like ngrok since the URI changes all the time
                //this way you don't need to modify the callback URL in the payload every time
                //ngrok changes the URI

                if (payload["callback"]["url"] != null)
                {
                    //localhost hostname can't work for callbacks so we won't overwrite it.
                    //this happens for example when testing with sign-in to an IDP and https://localhost is used as redirect URI
                    //in that case the callback should be configured in the payload directly instead of being modified in the code here
                    string host = GetRequestHostName();
                    if (!host.Contains("//localhost"))
                    {
                        payload["callback"]["url"] = String.Format("{0}:/api/AzADProof/issuanceCallback", host);
                    }
                    else
                    {
                        payload["callback"]["url"] = payload["callback"]["url"].ToString().Replace("https://YOURPUBLICREACHABLEHOSTNAME", _msvcOptions.VCCallbackHostURL);
                    }
                }

                // set our api-key in the request so we can check it in the callbacks we receive
                if (payload["callback"]["headers"]["api-key"] != null)
                {
                    payload["callback"]["headers"]["api-key"] = this._apiKey;
                }

                //get the manifest from the appsettings, this is the URL to the credential created in the azure portal. 
                //the display and rules file to create the credential can be dound in the credentialfiles directory
                //make sure the credentialtype in the issuance payload matches with the rules file
                //for this sample it should be VerifiedCredentialExpert
                if (payload["issuance"]["manifest"] != null)
                {
                    payload["issuance"]["manifest"] = _msvcOptions.CredentialManifest;
                }

                //here you could change the payload manifest and change the firstname and lastname
                //payload["issuance"]["claims"]["given_name"] = "Megan";
                //payload["issuance"]["claims"]["family_name"] = "Bowen";
                payload["issuance"]["claims"]["email"] = email;

                jsonString = JsonConvert.SerializeObject(payload);

                //CALL REST API WITH PAYLOAD
                HttpStatusCode statusCode = HttpStatusCode.OK;
                string response = null;

                try
                {
                    //The VC Request API is an authenticated API. We need to clientid and secret (or certificate) to create an access token which 
                    //needs to be send as bearer to the VC Request API
                    var accessToken = await GetAccessToken();
                    if (accessToken.Item1 == String.Empty)
                    {
                        _logger.LogError(String.Format("failed to acquire accesstoken: {0} : {1}"), accessToken.error, accessToken.error_description);
                        ModelState.AddModelError(accessToken.error, accessToken.error_description);
                        return ApiModelInvalid(ModelState);
                    }

                    //var client = _httpClientFactory.CreateClient();
                    HttpClient client = new HttpClient();
                    var defaultRequestHeaders = client.DefaultRequestHeaders;
                    defaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.token);

                    HttpResponseMessage res = await client.PostAsync(_msvcOptions.ApiEndpoint, new StringContent(jsonString, Encoding.UTF8, "application/json"));
                    response = await res.Content.ReadAsStringAsync();
                    statusCode = res.StatusCode;

                    if (statusCode == HttpStatusCode.Created)
                    {
                        _logger.LogTrace("succesfully called Request API");
                        JObject requestConfig = JObject.Parse(response);
                        if (newpin != null) { requestConfig["pin"] = newpin; }
                        requestConfig.Add(new JProperty("id", state));
                        jsonString = JsonConvert.SerializeObject(requestConfig);

                        //We use in memory cache to keep state about the request. The UI will check the state when calling the presentationResponse method

                        var cacheData = new
                        {
                            status = "notscanned",
                            message = "Request ready, please scan with Authenticator",
                            expiry = requestConfig["expiry"].ToString()
                        };
                        _cache.Set(state, JsonConvert.SerializeObject(cacheData));
                        var qrCodeBytes = QRCodeUtility.Create(requestConfig["url"].ToString());

                        var qrCode = Convert.ToBase64String(qrCodeBytes, 0, qrCodeBytes.Length);
                        var msLoginProofRequest = new AzLoginProofGetResponseModel(requestConfig["requestId"].ToString(), requestConfig["url"].ToString(), requestConfig["expiry"].ToString(), state, qrCode, newpin);

                        _context.MSLoginProofRequests.Add(msLoginProofRequest);
                        await _context.SaveChangesAsync();

                        return ApiOk(msLoginProofRequest);
                    }
                    else
                    {
                        _logger.LogError("Unsuccesfully called Request API");
                        ModelState.AddModelError(statusCode.ToString(), "API call did not return status 'Created'");
                        return ApiModelInvalid(ModelState);
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "AddEmail IssuanceRequest API Error");
                    ModelState.AddModelError("", "Something went wrong calling the API: " + ex.Message);
                    return ApiModelInvalid(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AddEmail IssuanceRequest Error");
                throw;
            }
        }

        /// <summary>
        /// This method is called by the VC Request API when the user scans a QR code and accepts the issued Verifiable Credential
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("IssuanceCallback")]
        public async Task<ActionResult> IssuanceCallback()
        {
            try
            {
                var message = string.Empty;
                var status = new AzLoginProofStatusModel();
                string content = await new System.IO.StreamReader(this.Request.Body).ReadToEndAsync();
                _logger.LogTrace("callback!: " + content);
                this.Request.Headers.TryGetValue("api-key", out var apiKey);
                if (this._apiKey != apiKey)
                {
                    _logger.LogTrace("api-key wrong or missing");
                    ModelState.AddModelError("", "api-key wrong or missing");
                    return ApiModelInvalid(ModelState);
                }
                JObject issuanceResponse = JObject.Parse(content);
                var state = issuanceResponse["state"].ToString();
                var requestId = issuanceResponse["requestId"].ToString();

                //there are 2 different callbacks. 1 if the QR code is scanned (or deeplink has been followed)
                //Scanning the QR code makes Authenticator download the specific request from the server
                //the request will be deleted from the server immediately.
                //That's why it is so important to capture this callback and relay this to the UI so the UI can hide
                //the QR code to prevent the user from scanning it twice (resulting in an error since the request is already deleted)
                if (issuanceResponse["code"].ToString() == "request_retrieved")
                {
                    var cacheData = new
                    {
                        status = "request_retrieved",
                        message = "QR Code is scanned. Waiting for issuance...",
                    };
                    status = new AzLoginProofStatusModel(requestId, issuanceResponse["code"].ToString(), state, content);
                    _context.MSLoginProofStatuses.Add(status);
                    await _context.SaveChangesAsync();
                    _cache.Set(state, JsonConvert.SerializeObject(cacheData));
                }

                //
                //This callback is called when issuance is completed.
                //
                if (issuanceResponse["code"].ToString() == "issuance_successful")
                {
                    var cacheData = new
                    {
                        status = "issuance_successful",
                        message = "Credential successfully issued",
                    };

                    status = new AzLoginProofStatusModel(requestId, issuanceResponse["code"].ToString(), state, content);
                    _context.MSLoginProofStatuses.Add(status);
                    await _context.SaveChangesAsync();
                    _cache.Set(state, JsonConvert.SerializeObject(cacheData));
                }
                //
                //We capture if something goes wrong during issuance. See documentation with the different error codes
                //
                if (issuanceResponse["code"].ToString() == "issuance_error")
                {
                    var cacheData = new
                    {
                        status = "issuance_error",
                        payload = issuanceResponse["error"]["code"].ToString(),
                        //at the moment there isn't a specific error for incorrect entry of a pincode.
                        //So assume this error happens when the users entered the incorrect pincode and ask to try again.
                        message = issuanceResponse["error"]["message"].ToString()

                    };

                    status = new AzLoginProofStatusModel(requestId, issuanceResponse["code"].ToString(), state, content);
                    _context.MSLoginProofStatuses.Add(status);
                    await _context.SaveChangesAsync();
                    _cache.Set(state, JsonConvert.SerializeObject(cacheData));
                }

                return ApiOk(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        [HttpGet]
        [Route("IssueStatus/{requestId}")]
        public async Task<IActionResult> GetIssueStatusAsync(string requestId)
        {
            var result = new MSProofStatus();
            var message = string.Empty;
            var statusCode = string.Empty;
            JObject issuanceResponse;
            try
            {
                var status = await _loginProofService.GetLoginProofStatusAsync(requestId);

                if (status.Code == "issuance_successful")
                {
                    message = "Credential successfully issued";
                    statusCode = "Accepted";
                    issuanceResponse = JObject.Parse(status.Json);
                    var state = issuanceResponse["state"].ToString();                    
                }
                else if (status.Code == "request_retrieved")
                {
                    message = "QR Code is scanned. Waiting for issuance...";
                }
                else if (status.Code == "no_status_yet")
                {
                    message = "Waiting for Issuance response...";
                    statusCode = "Waiting";
                }
                else if (status.Code == "issuance_error")
                {
                    issuanceResponse = JObject.Parse(status.Json);
                    message = issuanceResponse["error"]["message"].ToString();
                    statusCode = "Error";
                }
                else
                {
                    message = status.Code;
                    statusCode = "Unexpected";
                }
                result.Message = message;
                result.Status = statusCode;
                return ApiOk(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        //some helper functions
        protected async Task<(string token, string error, string error_description)> GetAccessToken()
        {
            // You can run this sample using ClientSecret or Certificate. The code will differ only when instantiating the IConfidentialClientApplication
            bool isUsingClientSecret = _msvcOptions.AppUsesClientSecret(_msvcOptions);

            // Since we are using application permissions this will be a confidential client application
            IConfidentialClientApplication app;
            if (isUsingClientSecret)
            {
                app = ConfidentialClientApplicationBuilder.Create(_msvcOptions.ClientId)
                    .WithClientSecret(_msvcOptions.ClientSecret)
                    .WithAuthority(new Uri(_msvcOptions.Authority))
                    .Build();
            }
            else
            {
                X509Certificate2 certificate = _msvcOptions.ReadCertificate(_msvcOptions.CertificateName);
                app = ConfidentialClientApplicationBuilder.Create(_msvcOptions.ClientId)
                    .WithCertificate(certificate)
                    .WithAuthority(new Uri(_msvcOptions.Authority))
                    .Build();
            }

            //configure in memory cache for the access tokens. The tokens are typically valid for 60 seconds,
            //so no need to create new ones for every web request
            app.AddDistributedTokenCache(services =>
            {
                services.AddDistributedMemoryCache();
                services.AddLogging(configure => configure.AddConsole())
                .Configure<LoggerFilterOptions>(options => options.MinLevel = Microsoft.Extensions.Logging.LogLevel.Debug);
            });

            // With client credentials flows the scopes is ALWAYS of the shape "resource/.default", as the 
            // application permissions need to be set statically (in the portal or by PowerShell), and then granted by
            // a tenant administrator. 
            string[] scopes = new string[] { _msvcOptions.VCServiceScope };

            AuthenticationResult result = null;
            try
            {
                result = await app.AcquireTokenForClient(scopes)
                    .ExecuteAsync();
            }
            catch (MsalServiceException ex) when (ex.Message.Contains("AADSTS70011"))
            {
                // Invalid scope. The scope has to be of the form "https://resourceurl/.default"
                // Mitigation: change the scope to be as expected
                return (string.Empty, "500", "Scope provided is not supported");
                //return BadRequest(new { error = "500", error_description = "Scope provided is not supported" });
            }
            catch (MsalServiceException ex)
            {
                // general error getting an access token
                return (String.Empty, "500", "Something went wrong getting an access token for the client API:" + ex.Message);
                //return BadRequest(new { error = "500", error_description = "Something went wrong getting an access token for the client API:" + ex.Message });
            }

            _logger.LogTrace(result.AccessToken);
            return (result.AccessToken, String.Empty, String.Empty);
        }
        protected string GetRequestHostName()
        {
            string scheme = "https";// : this.Request.Scheme;
            string originalHost = this.Request.Headers["x-original-host"];
            string hostname = "";
            if (!string.IsNullOrEmpty(originalHost))
                hostname = string.Format("{0}://{1}", scheme, originalHost);
            else hostname = string.Format("{0}://{1}", scheme, this.Request.Host);
            return hostname;
        }
        protected bool IsMobile()
        {
            string userAgent = this.Request.Headers["User-Agent"];

            if (userAgent.Contains("Android") || userAgent.Contains("iPhone"))
                return true;
            else
                return false;
        }
    }
}
