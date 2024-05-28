using Autofac;
using LePickaOrders.Application.Queries.Products;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using MediatR.Extensions.Autofac.DependencyInjection;
using Autofac.Extensions.DependencyInjection;
using LePickaOrders.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using LePickaOrders.Application.Modules;
using LePickaOrders.Infrastructure.MessageBus;

namespace LePickaOrders.API
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
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Orders API", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter JWT bearer token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id= "Bearer"
                            }
                        },
                        new string[]{ }
                    }
                });
            });

            var configuration = MediatRConfigurationBuilder
                .Create(typeof(GetProductQuery).Assembly)
                .WithAllOpenGenericHandlerTypesRegistered()
                .WithRegistrationScope(RegistrationScope.Scoped)
                .Build();
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
            {
                containerBuilder.RegisterMediatR(configuration);
                containerBuilder.RegisterModule<DataAccessModule>();
                containerBuilder.RegisterModule<AutoMapperModule>();
                containerBuilder.RegisterModule<MessageBusModule>();
                containerBuilder.Register(c =>
                {
                    var dbContextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                    if (builder.Environment.IsProduction())
                    {
                        Console.WriteLine(">>?orders");

                        StringBuilder sb = new StringBuilder();
                        sb.Append(builder.Configuration.GetConnectionString("DbConn")) //todo prod conn string
                            .Append(Environment.GetEnvironmentVariable("CONNECTION_STRING_CREDS")); //TODO on server

                        Console.WriteLine(sb.ToString());

                        dbContextOptionsBuilder.UseSqlServer(sb.ToString());
                    }

                    else if (builder.Environment.IsDevelopment())
                    {
                        dbContextOptionsBuilder
                        .UseSqlServer(builder.Configuration.GetConnectionString("DbConn"));
                    }



                    return new ApplicationDbContext(dbContextOptionsBuilder.Options);
                }).AsSelf().InstancePerLifetimeScope();
            });


            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSetting:key"])),
                    ClockSkew = TimeSpan.Zero
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
            PrepPopulation(app);

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        private static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<ApplicationDbContext>()!);
            }
        }

        private static void SeedData(ApplicationDbContext context)
        {
            Console.WriteLine("--> Attempting to apply migrations");
            try
            {
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not run migrations: {ex.Message}");
            }
        }
    }
}
