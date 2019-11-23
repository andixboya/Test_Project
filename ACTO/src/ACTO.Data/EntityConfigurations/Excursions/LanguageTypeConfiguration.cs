using ACTO.Data.Models;
using ACTO.Data.Models.Excursions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACTO.Data.EntityConfigurations.Excursions
{
    public class LanguageTypeConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.HasKey(lt => lt.Id);
        }
    }
}
