using CustomConfiguration.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomConfiguration.Context
{
    class ConfigurationEntityConfig : IEntityTypeConfiguration<ConfigurationEntity>
    {
        public void Configure(EntityTypeBuilder<ConfigurationEntity> builder)
        {
            builder.HasKey(c => c.Key);
        }
    }
}
