# ties4560-demo3

# ASP.NET Core 
## Creation
 * dotnet new webapi (https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-new-sdk-templates#webapi)
 * dotnet add package Microsoft.EntityFrameworkCore
 * dotnet add package Microsoft.EntityFrameworkCore.InMemory
## Exception handling
Exceptions are caught on a global middleware.

``` csharp
app.UseExceptionHandler(execptionHandler =>
{
  execptionHandler.Run(async context =>
  {
    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
    context.Response.ContentType = "application/json";
    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

    ErrorMessage msg = new ErrorMessage();

    if (exceptionHandlerPathFeature?.Error is Exception ex)
      msg.Reason = ex.Message;
    else
      msg.Reason = "Unexpected internal exception.";

    await context.Response.WriteAsync(JsonSerializer.Serialize(msg));
  });
});
```
