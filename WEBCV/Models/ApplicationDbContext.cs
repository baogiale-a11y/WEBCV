using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WEBCV.Models;
using WEBCV.Models;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Resume> Resumes { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<JobApplication> JobApplications { get; set; }
    public DbSet<Rating> Ratings { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Rating>()
            .HasOne(r => r.RatedUser)
            .WithMany()
            .HasForeignKey(r => r.RatedUserId)
            .OnDelete(DeleteBehavior.Restrict); // Thay đổi từ CASCADE thành RESTRICT

        builder.Entity<Rating>()
            .HasOne(r => r.RatingUser)
            .WithMany()
            .HasForeignKey(r => r.RatingUserId)
            .OnDelete(DeleteBehavior.Cascade); // Giữ nguyên CASCADE cho một trong hai mối quan hệ
    }
}