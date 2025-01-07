using Microsoft.EntityFrameworkCore;
using AuthService.Contracts;

namespace AuthService.Data;

public class AuthDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public AuthDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public AuthDbContext(DbContextOptions<AuthDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Users_pkey");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Username).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(100).IsRequired();
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
        });

        base.OnModelCreating(modelBuilder);
    }
}
