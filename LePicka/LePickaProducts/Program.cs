using Microsoft.EntityFrameworkCore;
using LePickaProducts.Infrastructure.DatabaseContext;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using LePickaProducts.Infrastructure.Database;
using LePickaProducts.Application;

namespace LePickaProducts
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var configuration = MediatRConfigurationBuilder
                .Create(typeof(QueryCommandRegistrationModule).Assembly)
                .WithAllOpenGenericHandlerTypesRegistered()
                .WithRegistrationScope(RegistrationScope.Scoped)
                .Build();
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
            {
                containerBuilder.RegisterMediatR(configuration);
                containerBuilder.RegisterModule<QueryCommandRegistrationModule>();
                containerBuilder.RegisterModule<DataAccessModule>();
                containerBuilder.Register(c =>
                {
                    var dbContextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                    dbContextOptionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("ProdsConn"));

                    return new ApplicationDbContext(dbContextOptionsBuilder.Options);
                }).AsSelf().InstancePerLifetimeScope();
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            PrepPopulation(app);


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
