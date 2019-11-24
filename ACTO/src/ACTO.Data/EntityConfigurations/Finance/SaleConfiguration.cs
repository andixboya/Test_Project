

namespace ACTO.Data.EntityConfigurations.Finance
{
    using ACTO.Data.Models.Finance;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.HasKey(s => s.Id);

            //unnecessary, because we`ll check for sales waitiing to be cached in by null of this!

            //builder.HasOne(s => s.Liquidation)
            //    .WithMany(l => l.ReportedSales)
            //    .HasForeignKey(s => s.LiquidationId);

            builder.HasOne(s => s.Representative)
                .WithMany(r => r.Sales)
                .HasForeignKey(s => s.RepresentativeId);
        }
    }
}
