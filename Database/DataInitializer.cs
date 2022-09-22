using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ties4560_demo3.Database
{
  public class DataInitializer
  {
    public static void Initialize()
    {
      using (var db = new NewsServiceContext())
      {
        db.Database.EnsureCreated();

        foreach (var category in Categories)
          db.Categories.Add(category);

        db.SaveChanges();
      }
    }

    #region FIELDS

    private static readonly List<Category> Categories = new List<Category>()
    {
      new Category(1, "News"),
      new Category(2, "Local"),
      new Category(3, "Global"),
      new Category(4, "Sports"),
      new Category(5, "Entertainment")
    };

    #endregion
  }
}
