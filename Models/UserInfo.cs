using static ties4560_demo3.Models.User;

namespace ties4560_demo3.Models
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
