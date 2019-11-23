

namespace ACTO.Data.EntityConfigurations.Excursions
{
    using ACTO.Data.Models;
    using ACTO.Data.Models.Excursions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class RepresentativeConfiguration : IEntityTypeConfiguration<Representative>
    {
        public void Configure(EntityTypeBuilder<Representative> builder)
        {
            builder.HasKey(r => r.Id);

            builder.HasOne(r => r.User)
                .WithOne(u => u.Representative)
                .HasForeignKey<Representative>(r=> r.UserId);
        }
    }
}
