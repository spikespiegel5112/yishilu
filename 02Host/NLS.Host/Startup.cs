using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using NLS.Configuration;
using NLS.Host.Web.Core;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NLS.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(p => { p.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); p.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss"; });

            services.AddCors(p =>
            {
                p.AddPolicy("NLSCors", c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(p =>
            {
                p.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = "NLS_XINYI",
                    ValidAudience = "NLS_XINYI",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("G0ae*$3c5nmahi4005pa2ea-0fea210c8tae")),
                    ClockSkew = TimeSpan.Zero,
                };
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info()
                {
                    Title = "NLS WEBAPI",
                    Version = "v1",
                    Description = "NLS WebApi 文档"
                });
                options.CustomSchemaIds(type => type.FullName);
                options.DocInclusionPredicate((p, d) => true);
                options.AddSecurityDefinition("Bearer",
                    new ApiKeyScheme
                    {
                        In = "header",
                        Description = "请输入Authorize接口返回的Token，前置Bearer。示例：Bearer {Token}",
                        Name = "Authorization",
                        Type = "apiKey"
                    });
                options.AddSecurityRequirement(
                    new Dictionary<string, IEnumerable<string>>
                    {
                        { "Bearer",
                          Enumerable.Empty<string>()
                        },
                    });

                //options.IncludeXmlComments(ConfigurationManager.AppSetting("XMLPath"), true);
            });

            return services.AddGlobalIoC();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.NLSUseStaticFiles("__StaticFile");

            app.UseCors("NLSCors");

            app.UseAuthentication();
            //app.UseHttpsRedirection();

            app.UseSwagger(c => { c.RouteTemplate = "swagger/{documentName}/swagger.json"; });

            app.UseSwaggerUI(options =>
            {
                options.ShowExtensions();
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "NLS API V1");
                options.DocExpansion(DocExpansion.List);
            });

            app.UseMvc();
        }
    }
}