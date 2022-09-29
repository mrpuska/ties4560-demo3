using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ties4560_demo3;
using ties4560_demo3.Authentication;
using ties4560_demo3.Controllers;
using ties4560_demo3.Database;
using ties4560_demo3.Exceptions;
using static ties4560_demo3.Models.User;

[assembly: ApiConventionType(typeof(ApiConventions))]
namespace ties4560_demo3
{
  public class Startup
  {
    public Startup (IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices (IServiceCollection services)
    {

      services.AddControllers();
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "ties4560_demo3", Version = "v1" });
        c.AddSecurityDefinition("Basic", new OpenApiSecurityScheme()
        {
          In = ParameterLocation.Header,
          Description = "Enter username and password",
          Name = "Authorization",
          Type = SecuritySchemeType.Http,
          Scheme = "Basic"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
          {
            {
              new OpenApiSecurityScheme
              {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Basic"
                }
              },
              new string[] {}
            }
          });
      });

      services.AddAuthentication("Basic")
        .AddScheme<BasicAuthenticationHandlerOptions, BasicAuthenticationHandler>("Basic", opts =>
        {
        });

      services.AddAuthorization(options =>
      {
        options.AddPolicy("Elevated", policy =>
          policy.RequireClaim(ClaimTypes.Role, UserRoleType.Admin.ToString(), UserRoleType.PowerUser.ToString()));

        options.FallbackPolicy = new AuthorizationPolicyBuilder()
          .RequireAuthenticatedUser()
          .Build();
      });

      DataInitializer.Initialize();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure (IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ties4560_demo3 v1"));
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthentication();

      app.UseAuthorization();

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
          else if (exceptionHandlerPathFeature?.Error is AccessForbiddenException forbiddenEx)
          {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            msg.Reason = forbiddenEx.Message;
          }
          else if (exceptionHandlerPathFeature?.Error is Exception ex)
          {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            msg.Reason = "Unhandled internal exception.";
          }

          await context.Response.WriteAsync(JsonSerializer.Serialize(msg));
        });
      });

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });

    }
  }
}
