using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ties4560_demo3.User;

namespace ties4560_demo3
{
  public class UserInfo
  {
    public UserInfo (User user)
    {
      this.Username = user.Username;
      this.Role = user.Role;
    }

    public string Username { get; set; }

    public UserRoleType Role { get; set; }

  }
}
