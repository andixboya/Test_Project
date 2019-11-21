using ACTO.Data.Models;
using ACTO.Data.Models.Excursion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACTO.Data.EntityConfigurations
{
    public class LanguageTypeConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.HasKey(lt => lt.Id);
        }
    }
}
