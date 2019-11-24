

namespace ACTO.Data.EntityConfigurations.Finance
{
    using ACTO.Data.Models.Finance;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {   
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);
        }
    }
}
