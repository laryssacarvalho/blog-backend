
using BlogApi.Application;
using BlogApi.Infrastructure;
using BlogApi.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BlogApi 
{
    static class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigurationManager configuration = builder.Configuration;

            builder.Services.AddApplicationConfiguration(configuration)
                .AddInfrastructureConfiguration(configuration)
                .AddWebApiConfiguration(configuration);

            var app = builder.Build();
            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            ApplyMigrations(app);

            app.Run();
        }

        private static void ApplyMigrations(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<BlogDbContext>();
            context.Database.Migrate();
        }
    }
}