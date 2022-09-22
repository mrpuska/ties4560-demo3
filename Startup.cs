using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ties4560_demo3;
using ties4560_demo3.Controllers;
using ties4560_demo3.Database;
using ties4560_demo3.Exceptions;

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

      app.UseAuthorization();

      app.UseExceptionHandler(execptionHandler =>
      {
        execptionHandler.Run(async context =>
        {
          context.Response.ContentType = "application/json";
          var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

          ErrorMessage msg = new ErrorMessage();

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

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
