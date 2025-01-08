using CartService.Data;
using CartService.Repositories;
using CartService.Abstractions;
using CartService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace CartService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Добавили логирование в контейнер DI
            builder.Services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Debug);  // Уровень логирования
            });

            // Add services to the container.          

            builder.Services.AddDbContext<CartDbContext>(options =>
                options.UseNpgsql("Host=localhost;Port=5432;Database=shop_db;Username=postgres;Password=1234"));

            builder.Services.AddScoped<ICartService, Services.CartService>();
            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddScoped<QueueMessageProcessor>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Логируем, что контейнер DI создан
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Application started.");

            using (var scope = app.Services.CreateScope())
            {
                logger.LogInformation("Scope created.");

                var messageProcessor = scope.ServiceProvider.GetRequiredService<QueueMessageProcessor>();
                logger.LogInformation("QueueMessageProcessor resolved from scope.");

                messageProcessor.StartListening();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}