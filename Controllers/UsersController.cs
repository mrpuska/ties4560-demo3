using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ties4560_demo3.Database;
using ties4560_demo3.Exceptions;
using static ties4560_demo3.User;

namespace ties4560_demo3.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UsersController 
    : ControllerBase
  {
    public UsersController (IActionDescriptorCollectionProvider actionDescriptorCollectionProvider) 
      : base(actionDescriptorCollectionProvider)
    {
    }

    // POST api/<UserController>
    [HttpPost]
    [AllowAnonymous]
    public UserInfo Post ([FromBody] User user)
    {
      if (!user.Validate())
        throw new BadHttpRequestException("Invalid data");

      if (user.Password.Length < 8)
        throw new BadHttpRequestException("Password must be at least 8 characters");

      if (user.Role != UserRoleType.User && !User.HasClaim(ClaimTypes.Role, UserRoleType.Admin.ToString()))
        throw new AccessForbiddenException("Access denied");

      using (var db = new NewsServiceContext())
      {
        user.Created = DateTime.Now;
        db.Users.Add(user);
        db.SaveChanges();
        return new UserInfo(user);
      }
    }

  }
}
