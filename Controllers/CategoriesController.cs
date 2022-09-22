using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using ties4560_demo3.Database;
using ties4560_demo3.Exceptions;

namespace ties4560_demo3.Controllers
{
  [Produces("application/json")]
  [ApiController]
  [Route("api/[controller]")]
  public class CategoriesController
    : ControllerBase
  {

    public CategoriesController (IActionDescriptorCollectionProvider actionDescriptorCollectionProvider) 
      : base(actionDescriptorCollectionProvider)
    {

    }

    [HttpGet]
    public IEnumerable<Category> Get ()
    {
      using (var db = new NewsServiceContext())
      {
        return db.Categories.ToList();
      }
    }

    [HttpGet("{id}")]
    public Category Get (int id)
    {
      using (var db = new NewsServiceContext())
      {
        if (db.Categories.Find(id) is Category category)
          return category;
        else
          throw new NotFoundException($"Category with id {id} not found.");
      }
    }

  }
}
