using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;

namespace Repositories.Configurations
{
    public class StudentConfigurations : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.Property(s => s.Id).IsRequired();
            builder.Property(s => s.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(s => s.MiddleNames).IsRequired(false).HasMaxLength(200);
            builder.Property(s => s.DateOfBirth).IsRequired();
            builder.Property(s => s.Surname).IsRequired().HasMaxLength(50);
        }
    }
}
