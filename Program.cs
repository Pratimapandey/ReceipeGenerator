using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ReceipeGenerator.Data;
using ReceipeGenerator.Model;
using ReceipeGenerator.Services.Implementation;
using ReceipeGenerator.Services.Interface;
using System.Text;

namespace ReceipeGenerator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
            


            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            _ = builder.Services.AddSwaggerGen(); 
            builder.Services.AddDbContext<ReceipeDbContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString")));
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
               .AddDefaultTokenProviders()
               .AddEntityFrameworkStores<ReceipeDbContext>();

            builder.Services.AddScoped<IReceipeService, ReceipeService>(); // Register the employee service
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ReceipeGenerator API", Version = "v1" });
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,

                        ValidAudience = configuration["Jwt:ValidAudience"], // Update configuration key
                        ValidIssuer = configuration["Jwt:ValidIssuer"], // Update configuration key
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"])) // Update configuration key
                    };
                });

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}