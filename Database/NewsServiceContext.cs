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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseInMemoryDatabase(nameof(NewsServiceContext));
    }

  }
}
