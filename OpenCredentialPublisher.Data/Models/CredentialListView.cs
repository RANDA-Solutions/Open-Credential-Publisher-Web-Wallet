using Microsoft.EntityFrameworkCore;

namespace OpenCredentialPublisher.Data.Models
{
    [Keyless]
    public class CredentialListView
    {
        public string Name { get; set; }
    }

    [Keyless]
    public class CredentialSearchView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CredentialName { get; set; }
        public string CredentialType { get; set; }
        public string CredentialDescription { get; set; }
        public string CredentialNarrative { get; set; }
    }
}
