using Microsoft.EntityFrameworkCore;

namespace OpenCredentialPublisher.Data.Contexts
{
    public class ConfigurationContext : DbContext
    {
        public ConfigurationContext(DbContextOptions options) : base(options) { }

       // public DbSet<ApplicationSetting> ApplicationSettings { get; set; }

    }
}
