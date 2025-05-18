using IdentityProvider.API.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace IdentityProvider.API.Models.Configurations
{
	public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
	{
		public void Configure(EntityTypeBuilder<RefreshToken> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Token).IsRequired().HasMaxLength(200);
			builder.Property(x => x.Device).HasMaxLength(300);
			builder.Property(x => x.CreatedByIp).HasMaxLength(100);
			builder.Property(x => x.RevokedByIp).HasMaxLength(100);
			builder.Property(x => x.ReplacedByToken).HasMaxLength(200);

			builder.HasIndex(x => x.Token).IsUnique();
		}
	}
}
