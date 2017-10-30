using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;

namespace api
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
            // Add Cors
            services.AddCors(o => o.AddPolicy("MoneyboardPolicy", builder =>
            {
                builder.SetIsOriginAllowed(uri => uri.StartsWith("http://localhost:56779"))
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            }));


            services.AddMvc();

            // Set Cors filter
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("MoneyboardPolicy"));
            });

            //services.AddDbContext<dal.MoneyboardContext>(options => options.UseInMemoryDatabase("Moneyboard"));
            services.AddDbContext<dal_postgres.MoneyboardPostgresContext>(options => options.UseNpgsql(string.Format(dal_postgres.MoneyboardPostgresContextContextFactory.CONNECTION_STRING, "localhost")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            // Astuce pour appeler une méthode de SeedData
            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            serviceScopeFactory.SeedData();

            // Cors
            app.UseCors("MoneyboardPolicy");
        }
    }
}
