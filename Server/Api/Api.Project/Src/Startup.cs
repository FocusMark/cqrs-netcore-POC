using Amazon.SimpleNotificationService;
using Amazon.XRay.Recorder.Handlers.AwsSdk;
using FocusMark.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using Microsoft.OpenApi.Models;
using System;

namespace FocusMark.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(options =>
            {
                var apiInfo = new OpenApiInfo
                {
                    Title = "FocusMark Project API",
                    Version = "v1"
                };
                
                options.SwaggerDoc("v1", apiInfo);
            });

            IConfiguration eventSourceConfiguration = this.Configuration.GetSection("AWS:SNS");
            services.Configure<EventSourceConfiguration>(eventSourceConfiguration);

            //AWSSDKHandler.RegisterXRay<IAmazonSimpleNotificationService>();
            services.AddTransient<IAmazonSimpleNotificationService, AmazonSimpleNotificationServiceClient>();

            services.AddTransient<ProjectEventSource>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseXRay("FocusMark.Api.Project", this.Configuration);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
