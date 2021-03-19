using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using t3winc.version.data;
using t3winc.version.common.Interfaces;
using t3winc.version.data.Repos;
using System.Reflection;
using System.IO;

namespace t3winc.version.api
{
    public class Startup
    {
        private static readonly string _env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        private static readonly string _connectName = (String.IsNullOrEmpty(_env) ? "DefaultConnection" : _env);
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Serilog.ILogger>(Log.Logger);
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "version.api", Version = "v1" });
                var fileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var filePath = Path.Combine(AppContext.BaseDirectory, fileName);
                c.IncludeXmlComments(filePath);
            });
            services.AddDbContext<VersionContext>(options => options.UseSqlServer(Configuration.GetConnectionString(_connectName)));
            services.AddScoped<IVersionRepo, VersionRepo>();
            services.AddScoped<IProductRepo, ProductRepo>();
            services.AddScoped<IBranchRepo, BranchRepo>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "version.api v1"));
            }

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
