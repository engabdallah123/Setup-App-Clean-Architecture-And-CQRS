
using Application;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure;
using Infrastructure.Dependency;
using Infrastructure.Repositories;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;


namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthorization();
            builder.Services.AddOpenApi();

            // Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
             .AddEntityFrameworkStores<ApplicationDBContext>()
             .AddDefaultTokenProviders();

            // layers
            builder.Services.AddApplication()
                            .AddInfrastructure(builder.Configuration);

            // add services 
            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // serilog
            builder.Host.UseSerilog((context,configuration) =>
                         configuration.ReadFrom.Configuration(context.Configuration));

            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            // Middleware
            app.UseHttpsRedirection();
            app.UseSerilogRequestLogging();
            app.UseCors("AllowAll");

            app.UseAuthorization();
            app.Run();
        }
    }
}
