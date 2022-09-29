using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ties4560_demo3.Models
{
  public class Link
  {

    public Link () { }

    public Link (string href, string rel, string method)
    {
      this.Href = href;
      this.Rel = rel;
      this.Method = method;
    }

    public string Href { get; set; }
    public string Rel { get; set; }
    public string Method { get; set; }
  }
}
