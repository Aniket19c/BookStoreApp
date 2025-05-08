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
using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using BookStore.Models.Context;
using Repository.Helper; 
using ConsumerService;
using Repository.Interface;
using Repository.Implementation;
using Business.Implementation;

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
                builder.Services.AddScoped<IBookRL, BookRLImpl>();
                builder.Services.AddScoped<IBookBL, BookBLImpl>();
                builder.Services.AddScoped<IAddressRL, AddressRLImpl>();
                builder.Services.AddScoped<IAddressBL, AddressBLImpl>();
                builder.Services.AddSingleton<JwtTokenHelper>();


                builder.Services.AddSingleton<RabbitMqProducer>();
                builder.Services.AddSingleton<RabbitMqConsumer>();
                builder.Services.AddHostedService(provider =>
                {
                    var consumer = provider.GetRequiredService<RabbitMqConsumer>();
                    return new RabbitMqBackgroundService(consumer);
                });


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
                builder.Services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = "BookStore API",
                        Version = "v1"
                    });

                    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                        Description = "Enter 'Bearer' followed by space and your JWT token.\n\nExample: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
                    });

                    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                    {
                        {
                            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                            {
                                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                                {
                                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                        }
                    });
                });

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
    public class RabbitMqBackgroundService : BackgroundService
    {
        private readonly RabbitMqConsumer _consumer;

        public RabbitMqBackgroundService(RabbitMqConsumer consumer)
        {
            _consumer = consumer;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Consume();
            return Task.CompletedTask;
        }
    }
}
