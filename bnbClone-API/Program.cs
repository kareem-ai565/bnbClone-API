
using bnbClone_API.Data;
using bnbClone_API.Repositories.Impelementations;
using bnbClone_API.Repositories.Implementations;
using bnbClone_API.Repositories.Interfaces;
using bnbClone_API.UnitOfWork;
using Microsoft.EntityFrameworkCore;

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

            // Add services to the container.

            builder.Services.AddControllers();

            // Repositories and Unit of Work
            builder.Services.AddScoped<IFavouriteRepo, FavouriteRepo>();
            builder.Services.AddScoped<IAvailabilityRepo, AvailabilityRepo>();
            builder.Services.AddScoped<IViolationRepo, ViolationRepo>();
            builder.Services.AddScoped<IBookingRepo, BookingRepo>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();


            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            //swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "bnbClone API v1");
                    options.RoutePrefix = "swagger"; 
                });
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
