using Microsoft.EntityFrameworkCore;
using NLog.Web;
using NLog;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using Business.Interface;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services;
using BookStore.BusinessLayer.Services;
using Repository_Layer.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

                
                builder.Services.AddDbContext<BookStoreDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

              
                builder.Services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
                    options.InstanceName = "BookStoreRedis_";
                });

              
                builder.Services.AddScoped<IUserRL, UserRLImpl>();
                builder.Services.AddScoped<IUserBL, UserBLImpl>();
                builder.Services.AddSingleton<JwtTokenHelper>();

                
                var jwtKey = builder.Configuration["JWT:SecretKey"];
                var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

                builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidIssuer = builder.Configuration["JWT:Issuer"],
                        ValidAudience = builder.Configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
                    };
                });

               
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

          
                app.UseAuthentication(); 
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
