using Microsoft.EntityFrameworkCore;
using Task_6_ChatApi.Core.Entities;

namespace Task_6_ChatApi.Core.Context;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Message> Messages { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }
}