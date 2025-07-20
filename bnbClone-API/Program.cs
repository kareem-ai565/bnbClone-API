
using bnbClone_API.Data;
using bnbClone_API.Repositories.Impelementations;
using bnbClone_API.Repositories.Interfaces;
using bnbClone_API.Services.Impelementations;
using bnbClone_API.Services.Interfaces;

using bnbClone_API.Stripe;
using bnbClone_API.UnitOfWork;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace bnbClone_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //==================================== DataBase ================================
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            //==============================================================================

            

            builder.Services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = int.MaxValue;
            });


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // Add services to the container.

            builder.Services.AddScoped<IUnitOfWork , UnitOfWork.UnitOfWork>();
            builder.Services.AddScoped<IPropertyAmenityService ,  PropertyAmenityService>();
            builder.Services.AddScoped<IAmenityService, AmenityService>();
            builder.Services.AddScoped<IPropertyCategoryService, PropertyCategoryService>();
            builder.Services.AddScoped<IhostVerificationService, hostVerificationService>();

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals;
                }); ;



            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddScoped<IUnitOfWork, bnbClone_API.UnitOfWork.UnitOfWork>();
            builder.Services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));
            builder.Services.AddScoped<IBookingService, BookingService>();

            //===============Stripe=========================
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

            var app = builder.Build();

            app.UseCors("AllowAll");


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
       
                app.UseSwaggerUI(option => option.SwaggerEndpoint("/openapi/v1.json", "v1"));
            }

                app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
