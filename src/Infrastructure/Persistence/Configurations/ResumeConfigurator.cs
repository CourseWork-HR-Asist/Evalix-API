using Domain.Resumes;
using Domain.Users;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ResumeConfigurator: IEntityTypeConfiguration<Resume>
{
    public void Configure(EntityTypeBuilder<Resume> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new ResumeId(x));
        
        builder.Property(x => x.UserId).HasConversion(x => x.Value, x => new UserId(x));
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("fk_resumes_users_id")
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(x => x.Url).IsRequired().HasColumnType("varchar(255)");
        builder.Property(x => x.FileName).IsRequired().HasColumnType("varchar(255)");
        builder.Property(x => x.CreatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())");
        
        builder.HasMany(x=> x.Evaluations)
            .WithOne(x => x.Resume)
            .HasForeignKey(x => x.ResumeId)
            .HasConstraintName("fk_applications_resume_id")
            .OnDelete(DeleteBehavior.Restrict);
    }
}