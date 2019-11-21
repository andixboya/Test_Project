

namespace ACTO.Data.EntityConfigurations
{
    using ACTO.Data.Models;
    using ACTO.Data.Models.Excursion;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class LanguageExcursionConfiguration : IEntityTypeConfiguration<LanguageExcursion>
    {
        public void Configure(EntityTypeBuilder<LanguageExcursion> builder)
        {
            builder.HasKey(le => new { le.ExcursionId, le.LanguageId });

            builder.HasOne(le => le.Excursion)
                .WithMany(e => e.LanguageExcursions)
                .HasForeignKey(le => le.ExcursionId);

            builder.HasOne(le => le.Language)
                .WithMany(e => e.Excursions)
                .HasForeignKey(le => le.LanguageId);
        }
    }
}
