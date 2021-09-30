using BookApi.Model;
using BookApi.Model.Interfaces;
using BookApi.Model.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
            services.AddControllers(
                setupAction =>
                {
                    // if the value is set to false, and the client
                    //  request a format not supported by the API,
                    //  the API will return a response in the
                    // default format (JSON)
                    setupAction.ReturnHttpNotAcceptable = true;
                })
                //adding support for XML
                .AddXmlDataContractSerializerFormatters()
                //configuring how the api controller attribute should behave
                .ConfigureApiBehaviorOptions(b => {
                    // will be executed when the model state is invalid
                    b.InvalidModelStateResponseFactory = context =>
                    {
                        // the RFC implementation for problem details when
                        //dealing with validation errors is a vakidation ProblemDetails object
                        

                        var problemDetailsFactory = context.HttpContext.RequestServices
                        .GetRequiredService<ProblemDetailsFactory>();

                        // getting an instance of the problem details
                        // then pass the current context and modelstate
                        // this will translate errors from the model state to the RFC format
                        var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(
                            context.HttpContext, context.ModelState);

                        //adding additional info to the format which are not added by default
                        problemDetails.Detail = "See the error fields for details.";
                        problemDetails.Instance = context.HttpContext.Request.Path;

                        // choosing the status code to return
                        // by default a 400 bad request is returned, unless there is a validation problem
                        // then we return a 422

                        // find out which status code to use
                        var actionExecutingContext = context as Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;
                        
                        //if there are model state errors and all arguments were correctly
                        // found and parsed, we are dealing with validation errors
                        if ((context.ModelState.ErrorCount > 0) &&
                            (actionExecutingContext?.ActionArguments.Count ==
                            context.ActionDescriptor.Parameters.Count))
                        {
                            problemDetails.Title = "https://book.com/modelvalidationproblem";
                            problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                            problemDetails.Detail = "One or more validation occured!";

                            return new UnprocessableEntityObjectResult(problemDetails)
                            {
                                ContentTypes = { "application/problem+json" }
                            };
                        };

                        // if one of the arguments was not found or wasnt correctly parsed
                        // we are dealing with a null/unparseable input
                        problemDetails.Status = StatusCodes.Status400BadRequest;
                        problemDetails.Title = "One or more errors on input occured.";

                        return new BadRequestObjectResult(problemDetails)
                        {
                            ContentTypes = { "application/problem+json" }
                        };
                    };
                })
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            //services.AddControllers();
                //.ConfigureApiBehaviorOptions(setupAction => setupAction.)

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IBook, BookServices>();
            services.AddScoped<IAuthor, AuthorServices>();
            services.AddScoped<IPublisher, PublisherServices>();
            services.AddScoped<ICourseLibraryRepository, CourseLibraryRepository>();


            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //API VERSIONING
            services.AddApiVersioning(
                apiVersion =>
                {
                    apiVersion.AssumeDefaultVersionWhenUnspecified = true;
                    apiVersion.DefaultApiVersion = new ApiVersion(1, 0);
                    apiVersion.ReportApiVersions = true;
                });

            services.AddVersionedApiExplorer(v => v.GroupNameFormat = "'v'VVV"); //version name and number
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen();   
           /*
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo {
                        Title = "BookApi",
                        Version = "v1",
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
                c.IncludeXmlComments(cmlCommentsFullPath);
            });
            */
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            // IApiVersionDescriptionProvider provider - needed to configure swagger in ConfigureSwaggerOptions.cs
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    foreach (var desc in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json",
                            desc.GroupName.ToUpperInvariant());
                    };
                });
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookApi v1"));
            }
            else
            {
                // error handling for all 500 status code exception
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault occured!, Try again later");
                    });
                });
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
