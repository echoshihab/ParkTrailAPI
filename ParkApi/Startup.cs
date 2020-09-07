using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ParkApi.Data;
using ParkApi.Models.Repository;
using ParkApi.Models.Repository.IRepository;
using AutoMapper;
using ParkApi.Models.ParkMapper;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ParkApi
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
            services.AddDbContext<ApplicationDbContext>(options=> 
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<INationalParkRepository, NationalParkRespository>();
            services.AddScoped<ITrailRepository, TrailRespository>();
            services.AddAutoMapper(typeof(ParkMappingProfile));
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen();

            //services.AddSwaggerGen(options =>
            //{
            //options.SwaggerDoc("ParkTrailAPISpec", new Microsoft.OpenApi.Models.OpenApiInfo()
            //{
            //    Title = "Park Trails API",
            //    Version = "1",
            //    Description= "Park Trails API",
            //    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
            //    {
            //        Email="admin@test.com",
            //        Name="Shihab Khan",
            //        Url=new Uri("https://github.com/echoshihab")
            //    },
            //    License= new Microsoft.OpenApi.Models.OpenApiLicense()
            //    {
            //        Name="MIT License",
            //        Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
            //    }

            //});
           

            //    var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //var cmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
            //options.IncludeXmlComments(cmlCommentsFullPath);
            //});
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var desc in provider.ApiVersionDescriptions)
                {
                 options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json",
                 desc.GroupName.ToUpperInvariant());
                }
                options.RoutePrefix = "";

            });
            //app.UseSwaggerUI(options =>
            //{
            //    //options.SwaggerEndpoint("/swagger/ParkNPAPISpec/swagger.json", "Park API (National Parks)");
            //    options.SwaggerEndpoint("/swagger/ParkTrailAPISpec/swagger.json", "Park Trails API");
            //    options.RoutePrefix = "";
            //});
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
