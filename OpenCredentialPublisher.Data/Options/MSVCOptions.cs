using Microsoft.Identity.Web;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
//using Microsoft.Identity.Web;

namespace OpenCredentialPublisher.Data.Options
{
    public class MSVCOptions
    {
        public const string Section = "MSVCSettings";
        /// <summary>
        /// instance of Azure AD, for example public Azure or a Sovereign cloud (Azure China, Germany, US government, etc ...)
        /// </summary>
        public string Instance { get; set; }
        /// <summary>
        /// URL of the client REST API endpoint, still need to use tenantID, use ApiEndpoint instead.
        /// </summary>
        public string Endpoint { get; set; }
        /// <summary>
        /// Web Api scope. With client credentials flows, the scopes is ALWAYS of the shape "resource/.default"
        /// FUTURE THIS WILL CHANGE TO MS GRAPH SCOPE
        /// </summary>
        public string VCServiceScope { get; set; }

        //public string CredentialManifest { get; set; }

        public string CredentialManifest { get; set; }
        /// <summary>
        /// localhost hostname can't work for callbacks so we will use the configured value in appsetttings.json in that case.
        /// this happens for example when testing with sign-in to an IDP and https://localhost is used as redirect URI
        /// </summary>
        public string VCCallbackHostURL { get; set; }

        public string IssuerAuthority { get; set; }

        public string VerifierAuthority { get; set; }
        /// <summary>
        /// The Tenant is:
        /// - either the tenant ID of the Azure AD tenant in which this application is registered (a guid)
        /// or a domain name associated with the tenant
        /// - or 'organizations' (for a multi-tenant application)
        /// </summary>
        public string TenantId { get; set; }
        /// <summary>
        /// Guid used by the application to uniquely identify itself to Azure AD
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// URL of the authority
        /// </summary>
        public string Authority
        {
            get
            {
                return $"https://login.microsoftonline.com/{TenantId}";
            }
        }
        public string ApiEndpoint
        {
            get
            {
                return String.Format(CultureInfo.InvariantCulture, Endpoint, TenantId);
            }
        }
        /// <summary>
        /// Client secret (application password)
        /// </summary>
        /// <remarks>client credential applications can authenticate with AAD through two mechanisms: ClientSecret
        /// (which is a kind of application password: this property)
        /// or a certificate previously shared with AzureAD during the application registration 
        /// (and identified by the CertificateName property belows)
        /// <remarks> 
        public string ClientSecret { get; set; }
        /// <summary>
        /// Name of a certificate in the user certificate store
        /// </summary>
        /// <remarks>client credential applications can authenticate with AAD through two mechanisms: ClientSecret
        /// (which is a kind of application password: the property above)
        /// or a certificate previously shared with AzureAD during the application registration 
        /// (and identified by this CertificateName property)
        /// <remarks> 
        public string CertificateName { get; set; }
        /// <summary>
        /// Checks if the sample is configured for using ClientSecret or Certificate. This method is just for the sake of this sample.
        /// You won't need this verification in your production application since you will be authenticating in AAD using one mechanism only.
        /// </summary>
        /// <param name="config">Configuration from appsettings.json</param>
        /// <returns></returns>
        public bool AppUsesClientSecret(MSVCOptions config)
        {
            string clientSecretPlaceholderValue = "[Enter here a client secret for your application]";
            string certificatePlaceholderValue = "[Or instead of client secret: Enter here the name of a certificate (from the user cert store) as registered with your application]";

            if (!String.IsNullOrWhiteSpace(config.ClientSecret) && config.ClientSecret != clientSecretPlaceholderValue)
            {
                return true;
            }

            else if (!String.IsNullOrWhiteSpace(config.CertificateName) && config.CertificateName != certificatePlaceholderValue)
            {
                return false;
            }

            else
                throw new Exception("You must choose between using client secret or certificate. Please update appsettings.json file.");
        }
        public X509Certificate2 ReadCertificate(string certificateName)
        {
            if (string.IsNullOrWhiteSpace(certificateName))
            {
                throw new ArgumentException("certificateName should not be empty. Please set the CertificateName setting in the appsettings.json", "certificateName");
            }
            CertificateDescription certificateDescription = CertificateDescription.FromStoreWithDistinguishedName(certificateName);
            DefaultCertificateLoader defaultCertificateLoader = new DefaultCertificateLoader();
            defaultCertificateLoader.LoadIfNeeded(certificateDescription);
            return certificateDescription.Certificate;
        }

    }
}
