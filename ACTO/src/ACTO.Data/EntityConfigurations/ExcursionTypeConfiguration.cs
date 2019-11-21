using ACTO.Data.Models;
using ACTO.Data.Models.Excursion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACTO.Data.EntityConfigurations
{
    public class ExcursionTypeConfiguration : IEntityTypeConfiguration<ExcursionType>
    {
        public void Configure(EntityTypeBuilder<ExcursionType> builder)
        {

            builder.HasKey(et => et.Id);

            //i`ll leave it within the excursion, because i feel confident with one to many instead of many to one
            //builder.HasMany(et => et.Excursions)
            //    .WithOne(e => e.ExcursionType);
        }
    }
}
