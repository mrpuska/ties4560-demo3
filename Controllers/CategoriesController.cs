using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ties4560_demo3.Controllers
{
  [Produces("application/json")]
  [ApiController]
  [Route("api/[controller]")]
  public class CategoriesController
    : ControllerBase
  {

    public CategoriesController ()
    {
    }

    [HttpGet]
    public IEnumerable<Category> Get ()
    {
      return this.Categories;
    }

    #region FIELDS

    private readonly List<Category> Categories = new List<Category>()
    {
      new Category(0, "News"),
      new Category(1, "Local"),
      new Category(2, "Global"),
      new Category(3, "Sports"),
      new Category(4, "Entertainment")
    };

    #endregion
  }
}
