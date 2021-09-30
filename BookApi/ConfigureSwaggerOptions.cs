using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BookApi
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        // Configuring swagger options
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var desc in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    desc.GroupName, new OpenApiInfo()
                    {
                        Title = $"Book API {desc.ApiVersion}",
                        Version = desc.ApiVersion.ToString(),
                        Description = "Book API for users to add books and authors",
                        Contact = new OpenApiContact
                        {
                            Name = "Anyanwu Raphael",
                            Email = "anyanwuraphaelc@gmail.com"
                        },
                        License = new OpenApiLicense
                        {
                            Name = "MIT License",
                            Url = new Uri("https://en.wikipedia.org/wiki/MIT_Lincense")
                        }
                    });

                var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; // XML file
                var cmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile); // the path to system, this get the path and the fil, C:\Users\SelfMade\source\repos\BookApi\bookapi.xml

                // including xml comments to swagger, check notes for details pn
                // xml documentation
                options.IncludeXmlComments(cmlCommentsFullPath);
            }            
        }
    }
}
