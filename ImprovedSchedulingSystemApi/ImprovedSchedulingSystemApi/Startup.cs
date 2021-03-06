﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImprovedSchedulingSystemApi.Models.CustomModelBinders;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.StaticFiles;
using Swashbuckle.AspNetCore.Swagger;

namespace ImprovedSchedulingSystemApi
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
            services.AddMvc(opts =>
            {
                opts.ModelBinderProviders.Insert(0, new ObjectIdBinderProvider()); // Adds the ObejctId model binder
            }).AddJsonOptions(opts =>
            {
                opts.SerializerSettings.Converters.Add(new ObjectIDJsonConverter());
            });




            services.AddCors();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyHeader();
                    });
            });
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowAllOrigins"));
            });


            services.Configure<FormOptions>(options => options.BufferBody = true);

            // Register the Swagger gebnerator
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Appointment API",
                    Version = "v1",
                    Description = "The Middleware API to access the backend mongodb service for the appointment system",
                    TermsOfService = "None"
                });

                // Set the comments path for the Swagger JSON and UI
                var basepath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basepath, "ImprovedSchedulingSystemApi.xml");
                c.IncludeXmlComments(xmlPath);

            });


        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles(); // Allows loading to index.html
            app.UseStaticFiles(); //Allows the application to use wwwroot for the files
            app.UseMvc(); //MVC for the api layer
            app.UseCors("AllowAllOrigins");

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Appointment API v1");
            });


        }
    }
}
