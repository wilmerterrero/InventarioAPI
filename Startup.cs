using System;
using System.IO;
using System.Linq;
using System.Reflection;
using AutoMapper;
using InventarioAPI.Context;
using InventarioAPI.Entities;
using InventarioAPI.Models;
using InventarioAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

/*
 * Convencion que permite que se puedan mostrar los valores de retorno de nuestra API
 */
[assembly: ApiConventionType(typeof(DefaultApiConventions))]

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
                options.UseSqlServer(Configuration.GetConnectionString("sqlserver")));

            services.AddControllers(config => {
                config.Conventions.Add(new ApiExplorerGroupPerVersionConvention());
            })
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

                config.SwaggerDoc("v2", new OpenApiInfo
                {
                    Version = "v2",
                    Title = "Inventario API V2",
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

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                config.IncludeXmlComments(xmlPath);
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
                config.SwaggerEndpoint("/swagger/v2/swagger.json", "Inventario API V2");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public class ApiExplorerGroupPerVersionConvention : IControllerModelConvention
        {
            public void Apply(ControllerModel controller)
            {
                // example: "Controllers.v1"
                var controllerNamespace = controller.ControllerType.Namespace;
                var apiVersion = controllerNamespace.Split('.').Last().ToLower();
                controller.ApiExplorer.GroupName = apiVersion;
            }
        }
    }
}
