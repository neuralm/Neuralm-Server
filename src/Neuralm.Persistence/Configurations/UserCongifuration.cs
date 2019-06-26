using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neuralm.Domain.Entities;

namespace Neuralm.Persistence.Configurations
{
    internal class ApplicationUserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(user => user.Id);

            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Username).IsRequired().HasMaxLength(64);
            builder.Property(p => p.TimestampCreated).HasDefaultValueSql("GetDate()");

            //builder.HasMany(user => user.OwnedRooms)
            //    .WithOne(room => room.Owner)
            //    .HasForeignKey(room => room.OwnerId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
