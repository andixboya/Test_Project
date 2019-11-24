

namespace ACTO.Data.EntityConfigurations.Excursions
{
    using ACTO.Data.Models;
    using ACTO.Data.Models.Excursions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(t => t.Id);

            builder.HasOne(t => t.Customer)
                .WithMany(c => c.Tickets)
                .HasForeignKey(t => t.CustomerId);

            builder.HasOne(t => t.Excursion)
                .WithMany(e => e.SoldTickets)
                .HasForeignKey(t => t.ExcursionId);

            builder.HasOne(t => t.TourLanguage)
                .WithMany(t => t.Tickets)
                .HasForeignKey(t => t.TourLanguageId);

            builder.HasOne(t => t.Representative)
                .WithMany(r => r.SoldTickets)
                .HasForeignKey(t => t.RepresentativeId);


        }
    }
}
