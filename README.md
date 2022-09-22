# ties4560-demo3

## Creation
 * dotnet new webapi (https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-new-sdk-templates#webapi)
 * dotnet add package Microsoft.EntityFrameworkCore
 * dotnet add package Microsoft.EntityFrameworkCore.InMemory

## Background
 * Application where machine can post News headlines
 * Use cases:
   * Users can view headlines
   * Users can view headlines by category
   * Users can comment on headlines
   * Users can modify comments
   * Users can delete comments
   
### Initial specification
![image](https://user-images.githubusercontent.com/94618990/191752611-ef5b6e8f-abef-4275-aebd-0cc10fd0bfee.png)

## Tech stack
 * ASP.NET Core
 * Entity Framework Core
 * Swatshbucke ASP.NET Core (For API documentation)

## API Controllers
 * HeadlinesController
 * CommentsController
 * CategoriesController

## Exception handling
Exceptions are caught on a global middleware.

``` csharp
app.UseExceptionHandler(execptionHandler =>
{
  execptionHandler.Run(async context =>
  {
    context.Response.ContentType = "application/json";
    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

    ErrorMessage msg = new ErrorMessage();

    // Catch different types of exceptions
    if (exceptionHandlerPathFeature?.Error is BadHttpRequestException badHttpRequestEx)
    {
      context.Response.StatusCode = StatusCodes.Status400BadRequest;
      msg.Reason = badHttpRequestEx.Message;
    }
    else if (exceptionHandlerPathFeature?.Error is NotFoundException notFoundEx)
    {
      context.Response.StatusCode = StatusCodes.Status404NotFound;
      msg.Reason = notFoundEx.Message;
    }
    else if (exceptionHandlerPathFeature?.Error is Exception ex)
    {
      context.Response.StatusCode = StatusCodes.Status500InternalServerError;
      msg.Reason = "Unhandled internal exception.";
    }

    await context.Response.WriteAsync(JsonSerializer.Serialize(msg));
  });
});
```
## Response types
Endpoint response types and status codes are mainly set by ApiConventions.
https://learn.microsoft.com/en-us/aspnet/core/web-api/advanced/conventions?view=aspnetcore-6.0

``` csharp
public static class ApiConventions
{
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status500InternalServerError)]
  [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
  public static void Get ()
  {
  }
...
```
``` csharp
[assembly: ApiConventionType(typeof(ApiConventions))]
namespace ties4560_demo3
{
  public class Startup
...
```
## Final API Documentation

Swagger generated documentation. CAn be found at https://localhost/swagger/index.html when running locally.
![image](https://user-images.githubusercontent.com/94618990/191754304-df828dd9-69e3-41a4-a240-9281f67febe8.png)
