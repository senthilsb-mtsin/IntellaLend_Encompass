using AppSettings;
using EncompassWrapperAPI.Helper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;

namespace EncompassWrapperAPI
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
            // services.AddScoped<CustomActionFilter>();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v2", new OpenApiInfo() { Title = "Encompass Wrapper", Version = "v2" });

                c.EnableAnnotations();

                c.OperationFilter<HeaderFilter>();

            });

            services.AddCors(o => o.AddPolicy("OpenPolicy", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));

            //services.AddTransient<ValidateRequest>();

            services.AddRouting(r => r.SuppressCheckForUnhandledSecurityMetadata = true);

            services.AddHttpClient(HttpClientFactoryConstant.RequestWithValidator, c =>
            {
                c.BaseAddress = new Uri($"{AppSettingJson.EncompassAPIURL}");
            });
            //.AddHttpMessageHandler<ValidateRequest>();

            services.AddHttpClient(HttpClientFactoryConstant.RequestWithoutValidator, c =>
            {
                c.BaseAddress = new Uri($"{AppSettingJson.EncompassAPIURL}");
            });
            services.Configure<IISServerOptions>(options =>
            {
                options.AutomaticAuthentication = false;
            });
            services.Configure<IISOptions>(options =>
            {
                options.ForwardClientCertificate = false;
            });
            //services.AddSingleton<SessionHelper>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("OpenPolicy");

            app.Use((context, next) =>
            {
                context.Items["__CorsMiddlewareInvoked"] = true;
                return next();
            });

            loggerFactory.AddLog4Net();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v2/swagger.json", "Encompass Wrapper");

            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
