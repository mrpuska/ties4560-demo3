using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ties4560_demo3.Models
{
  public class Comment
    : ModelBase
  {
    public long Id { get; internal set; }
    public long HeadlineId { get; internal set; }
    public string Text { get; set; }
    public long Likes { get; internal set; }

    public bool IsValid()
    {
      return !string.IsNullOrEmpty(this.Text);
    }
  }
}
