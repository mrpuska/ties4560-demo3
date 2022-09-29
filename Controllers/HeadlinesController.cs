using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using ties4560_demo3.Database;
using ties4560_demo3.Exceptions;
using ties4560_demo3.Models;

namespace ties4560_demo3.Controllers
{
  [Produces("application/json")]
  [Route("api/[controller]")]
  [ApiController]
  public class HeadlinesController 
    : ControllerBase
  {
    public HeadlinesController (IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
      : base(actionDescriptorCollectionProvider)
    {
    }

    #region HEADLINES

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Headline>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status500InternalServerError)]
    public IEnumerable<Headline> Get (int? category, int page)
    {
      if (page < 0)
        throw new BadHttpRequestException("Invalid page");

      int paging = 10;
      int skip = paging * page;

      using (var db = new NewsServiceContext())
      {
        List<Headline> headlines;
        if (category.HasValue)
          headlines = db.Headlines.Where(h => h.Category == category).Skip(skip).Take(paging).ToList();
        else
          headlines = db.Headlines.Skip(skip).Take(paging).ToList();

        headlines.ForEach(h => SetLinks(h, h.Category));
        return headlines;
      }
    }

    // GET api/<HeadlinesController>/5
    [HttpGet("{id}")]
    public Headline Get (long id)
    {
      using (var db = new NewsServiceContext())
      {
        if (db.Headlines.FirstOrDefault(h => h.Id == id) is Headline headline)
          return headline;

        throw new NotFoundException($"Headline with id {id} not found.");
      }
    }

    // POST api/<HeadlinesController>
    [HttpPost]
    [Authorize(Policy = "Elevated")]
    public Headline Post ([FromBody] Headline data)
    {
      if (!data.IsValid())
        throw new BadHttpRequestException("Invalid data");

      using (var db = new NewsServiceContext())
      {
        db.Headlines.Add(data);
        db.SaveChanges();
        return data;
      }
    }

    #endregion

    #region COMMENTS

    [HttpGet]
    [Route("{id}/comments")]
    public IEnumerable<Comment> GetComments (long id, int page) 
    {
      if (page < 0)
        throw new BadHttpRequestException("Invalid page");

      int paging = 10;
      int skip = paging * page;

      using (var db = new NewsServiceContext())
      {
        var comments = db.Comments.Where(c => c.HeadlineId == id).Skip(skip).Take(paging).ToList();
        return comments;
      }
    }

    [HttpPost]
    [Route("{id}/comments")]
    public Comment PostComment (long id, [FromBody] Comment comment)
    {
      if (!comment.IsValid())
        throw new BadHttpRequestException("Invalid data");

      using (var db = new NewsServiceContext())
      {
        if (db.Headlines.Find(id) is Headline headline)
        {
          comment.HeadlineId = headline.Id;
          db.Comments.Add(comment);
          db.SaveChanges();
          return comment;
        }
        throw new NotFoundException($"Headline with id {id} not found.");
      }
    }

    #endregion

    private void SetLinks(Headline headline, int categoryId)
    {
      headline.Links.Add(
        new Link("self", $"/api/headlines/{headline.Id}", "GET"));
      headline.Links.Add(
        new Link("category", $"/api/categories/{categoryId}", "GET"));
      headline.Links.Add(
        new Link("comments", $"/api/headlines/{headline.Id}/comments/", "GET"));
    }
  }
}
