

namespace ACTO.Data.EntityConfigurations.Finance
{
    using ACTO.Data.Models.Finance;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class RefundConfiguration : IEntityTypeConfiguration<Refund>
    {
        public void Configure(EntityTypeBuilder<Refund> builder)
        {
            builder.HasKey(r => r.Id);

            builder.HasOne(r => r.Sale)
                .WithMany(s => s.Refunds)
                .HasForeignKey(r => r.SaleId);

        }
    }
}
