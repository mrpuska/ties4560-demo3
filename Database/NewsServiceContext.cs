using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ties4560_demo3.Database
{
  public class NewsServiceContext
    : DbContext
  {
    public DbSet<Headline> Headlines { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseInMemoryDatabase(nameof(NewsServiceContext));
      optionsBuilder.EnableSensitiveDataLogging(true);
    }

  }
}
