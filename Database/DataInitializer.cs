using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ties4560_demo3.Models;

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

        foreach (var user in Users)
          db.Users.Add(user);

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

    private static readonly List<User> Users = new List<User>()
    {
      new User("TestUser", "userPassword", User.UserRoleType.User),
      new User("AdminUser", "adminPassword", User.UserRoleType.Admin)
    };

    #endregion
  }
}
