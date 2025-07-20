
using bnbClone_API.Data;
using AutoMapper;
using bnbClone_API.Helpers.MappingProfiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using bnbClone_API.Services.Implementations;
using bnbClone_API.Services.Interfaces;
using bnbClone_API.Repositories.Impelementations;
using bnbClone_API.Repositories.Implementations;
using bnbClone_API.Repositories.Interfaces;
using bnbClone_API.UnitOfWorks;
using bnbClone_API.Services.Impelementations;

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
            builder.Services.AddScoped<IPropertyRepo, PropertyRepo>();
            builder.Services.AddScoped<IPropertyImageRepo, PropertyImageRepo>();
            builder.Services.AddScoped<ICancellationPolicyRepo, CancellationPolicyRepo>();
            builder.Services.AddScoped<ICancellationPolicyService, CancellationPolicyService>();
            builder.Services.AddScoped<IPropertyImageService, PropertyImageService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IPropertyService, PropertyService>();


            builder.Services.AddControllers();

            // AutoMapper
            builder.Services.AddAutoMapper(cfg => cfg.AddProfile<PropertyProfile>());


            // ? Swagger setup
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();


            var app = builder.Build();

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
