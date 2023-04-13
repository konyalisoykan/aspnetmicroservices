using Basket.Api.Repositories.Interfaces;
using Basket.Api.Repositories;
using Microsoft.Extensions.Configuration;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Basket.Api.GrpcServices;
using Discount.Grpc.Protos;

namespace Basket.Api
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigurationManager Configuration = builder.Configuration; // allows both to access and to set up the config
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            // Add services to the container.
            builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>
                      (o => o.Address = new Uri(Configuration["GrpcSettings:DiscountUrl"]));
            builder.Services.AddScoped<DiscountGrpcService>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();
            /*
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
               
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                
            });
            */
            app.Run();
        }
    }
}