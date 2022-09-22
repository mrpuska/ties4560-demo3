using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ties4560_demo3.Database;
using ties4560_demo3.Exceptions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ties4560_demo3.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CommentsController 
    : ControllerBase
  {
    public CommentsController (IActionDescriptorCollectionProvider actionDescriptorCollectionProvider) 
      : base(actionDescriptorCollectionProvider)
    {
    }

    // PUT api/<CommentsController>/5
    [HttpPut("{id}")]
    public Comment Put (long id, [FromBody] Comment data)
    {
      if (!data.IsValid())
        throw new BadHttpRequestException("Invalid data");

      using(var db = new NewsServiceContext())
      {
        if (db.Comments.Find(id) is Comment comment)
        {
          comment.Text = data.Text;
          db.SaveChanges();
          return comment;
        }

        throw new NotFoundException($"Comment with id {id} not found.");
      }
    }

    // DELETE api/<CommentsController>/5
    [HttpDelete("{id}")]
    public IActionResult Delete (long id)
    {
      using (var db = new NewsServiceContext())
      {
        if (db.Comments.Find(id) is Comment comment)
        {
          db.Comments.Remove(comment);
          db.SaveChanges();
          return this.Ok();
        }
        else
          throw new NotFoundException($"Comment with id {id} not found.");
      }
    }
  }
}
