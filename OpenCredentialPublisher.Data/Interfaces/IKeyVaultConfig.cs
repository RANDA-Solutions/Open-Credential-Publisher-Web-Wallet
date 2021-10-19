namespace OpenCredentialPublisher.Data.Interfaces
{ 
    public interface IKeyVaultConfig
    {
        string KeyVaultName { get; }

        string KeyVaultCertificateName { get; }

        int KeyVaultRolloverHours { get; }
    }
}
