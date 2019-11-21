

namespace ACTO.Data.EntityConfigurations
{
    using ACTO.Data.Models;
    using ACTO.Data.Models.Excursion;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class ExcursionConfiguration : IEntityTypeConfiguration<Excursion>
    {
        public void Configure(EntityTypeBuilder<Excursion> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.ExcursionType)
                .WithMany(et => et.Excursions)
                .HasForeignKey(e => e.ExcursionTypeId);
        }
    }
}
