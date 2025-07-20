using bnbClone_API.Data;
using bnbClone_API.Infrastructure;
using bnbClone_API.Models;
using bnbClone_API.Repositories;
using bnbClone_API.Repositories.Impelementations;
using bnbClone_API.Repositories.Impelementations.admin;
using bnbClone_API.Repositories.Implementations;
using bnbClone_API.Repositories.Implementations.admin;
using bnbClone_API.Repositories.Interfaces;
using bnbClone_API.Repositories.Interfaces.admin;
using bnbClone_API.Services.Impelementations;
using bnbClone_API.Services.Implementations;
using bnbClone_API.Services.Interfaces;
using bnbClone_API.Stripe;
using bnbClone_API.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Stripe;
using System.Text;
using IUnitOfWork = bnbClone_API.Infrastructure.IUnitOfWork;

namespace bnbClone_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // ----------------------
            // Database Configuration
            // ----------------------
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ----------------------
            // Identity & Auth Setup
            // ----------------------
            builder.Services.AddScoped<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["JWT:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["JWT:Audience"],
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            // ----------------------
            // Repository Registrations
            // ----------------------
            builder.Services.AddScoped<IUnitOfWork, bnbClone_API.Infrastructure.UnitOfWork>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IHostRepository, HostRepository>();
            builder.Services.AddScoped<IGenericRepository<ApplicationUser>, GenericRepository<ApplicationUser>>();
            builder.Services.AddScoped<IGenericRepository<Models.Host>, GenericRepository<Models.Host>>();




            // ----------------------
            // Admin Repository Registrations
            // ----------------------
            builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
            builder.Services.AddScoped<IViolationRepository, ViolationRepository>();
            builder.Services.AddScoped<IHostVerificationRepository, HostVerificationRepository>();
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

            // ----------------------
            // Service Registrations
            // ----------------------
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ITokenService, Services.Impelementations.TokenService>();
            builder.Services.AddScoped<IAdminUserService, AdminUserService>();
            builder.Services.AddScoped<IAdminPropertyService, AdminPropertyService>();
            builder.Services.AddScoped<IAdminViolationService, AdminViolationService>();
            builder.Services.AddScoped<IAdminHostVerificationService, AdminHostVerificationService>();
            builder.Services.AddScoped<IAdminNotificationService, AdminNotificationService>();
            builder.Services.AddScoped<IAdminDashboardService, AdminDashboardService>();

            builder.Services.AddScoped<IProfileService, ProfileService>();

            //==================================== DataBase ================================
          //  builder.Services.AddDbContext<ApplicationDbContext>(options =>
   // options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
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



          //  builder.Services.AddScoped<IUnitOfWork , UnitOfWork.UnitOfWork>();
            builder.Services.AddScoped<IPropertyAmenityService ,  PropertyAmenityService>();
            builder.Services.AddScoped<IAmenityService, AmenityService>();
            builder.Services.AddScoped<IPropertyCategoryService, PropertyCategoryService>();
            builder.Services.AddScoped<IhostVerificationService, hostVerificationService>();

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals;
                }); ;



    
            // Repositories and Unit of Work
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddScoped<IFavouriteRepo, FavouriteRepo>();
            builder.Services.AddScoped<IAvailabilityRepo, AvailabilityRepo>();
            builder.Services.AddScoped<IViolationRepo, ViolationRepo>();
            builder.Services.AddScoped<IUnitOfWork, bnbClone_API.Infrastructure.UnitOfWork>();
            builder.Services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));
            builder.Services.AddScoped<IBookingService, BookingService>();

            //===============Stripe=========================
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];



            //swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "bnbClone API",
                    Version = "v1"
                });

                // 🔐 Swagger JWT Setup
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.\r\n\r\nEnter 'Bearer' followed by your token.\r\nExample: \"Bearer eyJhb...\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });

            // ----------------------
            // App Pipeline
            // ----------------------
            var app = builder.Build();


            app.UseCors("AllowAll");


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
       
                app.UseSwaggerUI(option => option.SwaggerEndpoint("/openapi/v1.json", "v1"));
            }

                app.UseHttpsRedirection();


            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

