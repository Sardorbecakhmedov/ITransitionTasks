using Microsoft.EntityFrameworkCore;
using Task_4_Core.Entities;

namespace Task_4_Core.ApplicationDbContext;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }
}