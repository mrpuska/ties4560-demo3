using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ties4560_demo3.Database;

namespace ties4560_demo3.Controllers
{
  [Produces("application/json")]
  [Route("api/[controller]")]
  [ApiController]
  public class HeadlinesController 
    : ControllerBase
  {
    [HttpGet]
    public IEnumerable<Headline> Get ()
    {
      using (var db = new NewsServiceContext())
      {
        return db.Headlines.ToList();
      }
    }

    // GET api/<HeadlinesController>/5
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Headline), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status404NotFound)]
    public IActionResult Get (int id)
    {
      using (var db = new NewsServiceContext())
      {
        if (db.Headlines.FirstOrDefault(h => h.Id == id) is Headline headline)
          return this.Ok(headline);

        throw new Exception($"Headline with id {id} not found.");
      }
    }

    // POST api/<HeadlinesController>
    [HttpPost]
    public IActionResult Post ([FromBody] Headline data)
    {
      if (!data.IsValid())
        return this.BadRequest(new ErrorMessage("Invalid data"));

      using (var db = new NewsServiceContext())
      {
        db.Headlines.Add(data);
        db.SaveChanges();
        return this.Ok(data);
      }
    }

  }
}
