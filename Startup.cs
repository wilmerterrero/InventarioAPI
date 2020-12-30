using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InventarioAPI.Context;
using InventarioAPI.Entities;
using InventarioAPI.Models;
using InventarioAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace CientesAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(configuration =>
            {
                configuration.CreateMap<Cliente, ClienteDTO>();
                configuration.CreateMap<ClienteCreacionDTO, Cliente>();
                configuration.CreateMap<ProductoDTO, Producto>();
                configuration.CreateMap<ProveedorDTO, Proveedor>();
            }, typeof(Startup));

            services.AddTransient<IClienteService, ClienteService>();
            services.AddTransient<IProductoService, ProductoService>();
            services.AddTransient<IProveedorService, ProveedorService>();

            services.AddDbContext<AppDbContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("SQL_SERVER")));

            services.AddControllers()
                .AddNewtonsoftJson(options => options
                .SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo { 
                    Version = "v1", 
                    Title = "Inventario API V1",
                    Description = "Inventory api only for practice purposes",
                    TermsOfService = new Uri("https://wilmerterrero.com"),
                    License = new OpenApiLicense()
                    {
                        Name = "MIT",
                        Url = new Uri("https://wilmerterrero.com")
                    },
                    Contact = new OpenApiContact()
                    {
                        Name = "Wilmer Terrero",
                        Email = "wilmerterrero@test.com",
                        Url = new Uri("https://wilmerterrero.com")
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger(config =>
            {
                config.SerializeAsV2 = true;
            });

            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "Inventario API V1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
