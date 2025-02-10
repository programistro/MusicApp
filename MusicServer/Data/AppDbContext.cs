using Microsoft.EntityFrameworkCore;
using MusicServer.Models;

namespace MusicServer.Data;

public class AppDbContext : DbContext
{
    public DbSet<FileUser> FileUsers  => Set<FileUser>();
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=database.db");
    }
}