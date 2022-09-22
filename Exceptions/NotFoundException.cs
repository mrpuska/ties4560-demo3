using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ties4560_demo3.Exceptions
{
  public class NotFoundException
    : Exception
  {
    public NotFoundException(string message)
      :base(message) { }
  }
}
