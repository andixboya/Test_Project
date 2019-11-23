

namespace ACTO.Data.EntityConfigurations.Finance
{
    using ACTO.Data.Models.Finance;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class LiquidationConfiguration : IEntityTypeConfiguration<Liquidation>
    {
        public void Configure(EntityTypeBuilder<Liquidation> builder)
        {
            builder.HasKey(l => l.Id);

            builder.HasOne(l => l.Representative)
                .WithMany(r => r.Liquidations)
                .HasForeignKey(l => l.RepresentativeId);
        }
    }
}
