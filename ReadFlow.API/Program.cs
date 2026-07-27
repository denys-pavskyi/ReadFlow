using System.Reflection;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ReadFlow.API.Filters;
using ReadFlow.API.Middleware;
using ReadFlow.API.Models;
using ReadFlow.BLL.Behaviors;
using ReadFlow.BLL.Commands.Books;
using ReadFlow.BLL.Mappings;
using ReadFlow.BLL.Services;
using ReadFlow.BLL.Validators.Books;
using ReadFlow.DAL.Data;
using ReadFlow.DAL.Extensions;
using ReadFlow.DAL.Repositories.Implementations;
using ReadFlow.DAL.Repositories.Interfaces;

namespace ReadFlow.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Database
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Unit of Work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Services
            builder.Services.AddScoped<AnalyticsService>();

            // MediatR
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(CreateBookCommand).Assembly);
            });

            // FluentValidation
            builder.Services.AddValidatorsFromAssembly(typeof(CreateBookCommandValidator).Assembly);

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(BookMappingProfile).Assembly);

            // Pipeline Behaviors
            builder.Services.AddTransient(typeof(MediatR.IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            builder.Services.AddTransient(typeof(MediatR.IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<DatabaseExceptionFilter>();
                options.Filters.Add<ValidationExceptionFilter>();
                options.Filters.Add<DomainExceptionFilter>();
            })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                });

            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ReadFlow API",
                    Version = "v1",
                    Description = "API for ReadFlow - Book rating and review platform",
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "ReadFlow API v1");
                });
            }

            app.UseStatusCodePages(async context =>
            {
                var response = context.HttpContext.Response;

                if (response.StatusCode == 404)
                {
                    await response.WriteAsJsonAsync(new ErrorResponse
                    {
                        Message = "The requested resource was not found",
                        Code = "NotFound",
                        TraceId = context.HttpContext.TraceIdentifier,
                        Timestamp = DateTime.UtcNow
                    });
                }
                else if (response.StatusCode == 401)
                {
                    await response.WriteAsJsonAsync(new ErrorResponse
                    {
                        Message = "Unauthorized access",
                        Code = "Unauthorized",
                        TraceId = context.HttpContext.TraceIdentifier,
                        Timestamp = DateTime.UtcNow
                    });
                }
            });

            app.UseMiddleware<RequestLoggingMiddleware>();

            app.UseHttpsRedirection();
            app.UseCors("AllowFrontend");

            app.UseAuthorization();

            app.MapControllers();

            if (app.Environment.IsDevelopment())
            {
                await app.Services.SeedDatabaseAsync();
            }

            app.Run();
        }
    }
}
