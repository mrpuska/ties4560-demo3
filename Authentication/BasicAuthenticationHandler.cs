using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using ties4560_demo3.Database;

namespace ties4560_demo3.Authentication
{
  public class BasicAuthenticationHandler
    : AuthenticationHandler<BasicAuthenticationHandlerOptions>
  {
    public BasicAuthenticationHandler (IOptionsMonitor<BasicAuthenticationHandlerOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) 
      : base(options, logger, encoder, clock)
    {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync ()
    {
      HttpRequest request = this.Context.Request;
      AuthenticationHeaderValue authorization;

      try
      {
         authorization = AuthenticationHeaderValue.Parse(request.Headers["Authorization"]);
      }
      catch
      {
        // No authentication
        return AuthenticateResult.Fail("Authorization not provided");
      }

      // No authentication
      if (authorization == null)
        return AuthenticateResult.Fail("Authorization not provided");

      // No authentication (by this type)
      if (authorization.Scheme != "Basic")
        return AuthenticateResult.Fail("Authorization not provided");

      if (string.IsNullOrEmpty(authorization.Parameter))
      {
        return AuthenticateResult.Fail("Missing credentials");
      }

      var usernameAndPassword = this.ExtractUsernameAndPassword(authorization.Parameter);

      if (usernameAndPassword == null)
      {
        return AuthenticateResult.Fail("Invalid username or password");
      }

      var principal = await this.Authenticate(usernameAndPassword.Item1, usernameAndPassword.Item2);
      if (principal == null)
      {
        return AuthenticateResult.Fail("Invalid username or password");
      }

      // Authentication succesful
      return AuthenticateResult.Success(new AuthenticationTicket(principal, "Basic"));
    }

    protected override async Task HandleChallengeAsync (AuthenticationProperties properties)
    {
      Response.StatusCode = StatusCodes.Status401Unauthorized;
      Response.ContentType = "application/json";
      ErrorMessage error = new ErrorMessage("Unauthorized");
      await Microsoft.AspNetCore.Http.HttpResponseWritingExtensions.WriteAsync(Response, JsonSerializer.Serialize(error));
      await Task.FromResult(0);
    }

    protected override async Task HandleForbiddenAsync (AuthenticationProperties properties)
    {
      Response.StatusCode = StatusCodes.Status403Forbidden;
      Response.ContentType = "application/json";
      ErrorMessage error = new ErrorMessage("Access denied");
      await Microsoft.AspNetCore.Http.HttpResponseWritingExtensions.WriteAsync(Response, JsonSerializer.Serialize(error));
      await Task.FromResult(0);
    }

    private async Task<ClaimsPrincipal> Authenticate(string username, string password)
    {
      using (var db = new NewsServiceContext())
      {
        var user = db.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

        if (user == null)
          return null;

        ClaimsIdentity identity = new ClaimsIdentity("Basic");
        identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
        identity.AddClaim(new Claim(ClaimTypes.Role, user.Role.ToString()));

        return new ClaimsPrincipal(identity);
      }
    }

    private Tuple<string, string> ExtractUsernameAndPassword (string authorizationParameter)
    {
      try
      {
        // Get credentials from request
        var decodedCredentials = Convert.FromBase64String(authorizationParameter);
        var credentialsString = System.Text.Encoding.ASCII.GetString(decodedCredentials);
        var credentialsSplit = credentialsString.Split(':');
        if (credentialsSplit.Length == 2)
          return new Tuple<string, string>(credentialsSplit[0], credentialsSplit[1]);
        else
          return null;
      }
      catch (Exception ex)
      {
        return null;
      }
    }
  }
}
