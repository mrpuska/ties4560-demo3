using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ties4560_demo3
{
  public class Headline
  {
    public long Id { get; internal set; }

    public int Category { get; set; }

    public string Title { get; set; }

    public string Href { get; set; }

    public IEnumerable<object> Links { get; set; }

    public bool IsValid()
    {
      return !string.IsNullOrEmpty(this.Title) && !string.IsNullOrEmpty(this.Href);
    }

  }
}
