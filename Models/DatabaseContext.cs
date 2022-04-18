using Microsoft.EntityFrameworkCore;

namespace TestTask.Models;

public class DatabaseContext : DbContext
{
    public DbSet<Project> Projects { get; set; }
    
    public DbSet<ProjectTask> Tasks { get; set; }

    protected DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }
}