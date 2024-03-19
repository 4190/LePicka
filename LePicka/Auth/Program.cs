
using Auth.AsyncDataServices;
using Auth.Data;
using Auth.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Auth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            if (builder.Environment.IsProduction()) 
            {
                Console.WriteLine(">>?auth");

                StringBuilder sb = new StringBuilder();
                sb.Append(builder.Configuration.GetConnectionString("AuthConn"))
                    .Append(Environment.GetEnvironmentVariable("CONNECTION_STRING_CREDS"));

                Console.WriteLine(sb.ToString());

                builder
                    .Services
                    .AddDbContext<ApplicationDbContext>
                    (opt => opt.UseSqlServer(sb.ToString()));
            }
            
            else if(builder.Environment.IsDevelopment())
            { 
                builder
                    .Services
                    .AddDbContext<ApplicationDbContext>
                    (opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("AuthConn"))); 
            }

            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            builder.Services.AddScoped<IManageJWTService, ManageJWTService>();
            builder.Services.AddScoped<IMessageBusClient, MessageBusClient>();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders", corsbuilder =>
                {
                    corsbuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSetting:key"]!)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (bool.Parse(builder.Configuration["UseSwagger"]!))
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseCors();
            PrepDb.PrepPopulation(app);
            app.MapControllers();

            app.Run();
        }
    }
}
