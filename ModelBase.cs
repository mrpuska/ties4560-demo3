using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ties4560_demo3
{
  public abstract class ModelBase
  {
    [NotMapped]
    public List<Link> Links { get; internal set; } = new List<Link>();
  }
}
