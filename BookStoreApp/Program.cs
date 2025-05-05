using Microsoft.EntityFrameworkCore;
using NLog.Web;
using NLog;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace BookStoreApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
           
            var logger = NLog.LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();

            try
            {
                var builder = WebApplication.CreateBuilder(args);

                builder.Logging.ClearProviders();
                builder.Logging.SetMinimumLevel(LogLevel.Information);
                builder.Host.UseNLog();

                builder.Host.UseNLog();

              
                builder.Services.AddDbContext<BookStoreDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

              
                builder.Services.AddControllers();

                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                var app = builder.Build();

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
            catch (Exception ex)
            { 
                logger.Error(ex, "An exception occurred during application startup");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }
    }
}
