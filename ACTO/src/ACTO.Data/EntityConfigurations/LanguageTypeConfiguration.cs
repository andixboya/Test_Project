using ACTO.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACTO.Data.EntityConfigurations
{
    public class LanguageTypeConfiguration : IEntityTypeConfiguration<LanguageType>
    {
        public void Configure(EntityTypeBuilder<LanguageType> builder)
        {
            throw new NotImplementedException();
        }
    }
}
