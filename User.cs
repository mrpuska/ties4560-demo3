using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ties4560_demo3
{
  public class User
  {

    public User () { }

    public User(string username, string password, UserRoleType role)
    {
      Username = username;
      Password = password;
      Role = role;
    }

    public long Id { get; internal set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public DateTime Created { get; internal set; }

    public UserRoleType Role { get; set; }

    public class UserRole
    {

      public UserRole () { }

      public UserRoleType Type { get; set; }
    }

    public enum UserRoleType
    {
      User = 0,
      PowerUser = 1,
      Admin = 2
    }

    public bool Validate()
    {
      return !string.IsNullOrEmpty(this.Username) &&
        !string.IsNullOrEmpty(this.Password);
    }

  }
}
