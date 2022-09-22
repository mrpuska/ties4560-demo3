using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ties4560_demo3.Controllers
{
  public class ControllerBase 
    : Microsoft.AspNetCore.Mvc.ControllerBase
  {

    public ControllerBase (IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
    {
      routes = actionDescriptorCollectionProvider.ActionDescriptors.Items;
    }

    internal Link UrlLink (string relation, string routeName, object values)
    {
      var route = routes.FirstOrDefault(f => f.AttributeRouteInfo.Name != null &&
                              f.AttributeRouteInfo.Name.Equals(routeName));
      if (route == null)
        return new Link();

      var method = route.ActionConstraints.
                              OfType<HttpMethodActionConstraint>()
                              .First()
                              .HttpMethods
                              .First();
      var url = Url.Link(routeName, values).ToLower();
      return new Link(url, relation, method);
    }

    #region FIELDS

    private readonly IReadOnlyList<ActionDescriptor> routes;

    #endregion
  }
}
