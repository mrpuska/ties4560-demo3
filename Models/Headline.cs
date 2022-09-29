using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ties4560_demo3.Models
{
  public class Headline
    : ModelBase
  {
    public long Id { get; internal set; }

    public int Category { get; set; }

    public string Title { get; set; }

    public string Link { get; set; }

    public bool IsValid()
    {
      return !string.IsNullOrEmpty(this.Title) && !string.IsNullOrEmpty(this.Link);
    }

  }
}
