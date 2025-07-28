

﻿using bnbClone_API.Data;
using bnbClone_API.Data;
using bnbClone_API.Helpers.MappingProfiles;
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
using bnbClone_API.Stripe;
using bnbClone_API.UnitOfWork;
using bnbClone_API.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Stripe;
using System.Text;
using TokenService = bnbClone_API.Services.Impelementations.TokenService;


namespace bnbClone_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // ----------------------
            // Database Configuration
            //// ----------------------
            //builder.Services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ----------------------
            // Identity & Auth Setup
            // ----------------------
            builder.Services
    .AddIdentity<ApplicationUser, IdentityRole<int>>(o =>
    {
        o.Password.RequiredLength = 6;
        o.Password.RequireNonAlphanumeric = false;
        o.Password.RequireUppercase = false;
        o.Password.RequireLowercase = false;
        o.Password.RequireDigit = false;
        o.Password.RequiredUniqueChars = 0;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

            builder.Services.AddScoped<UserManager<ApplicationUser>>();
            builder.Services.AddScoped<SignInManager<ApplicationUser>>();



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


            // ----------------------
            // host Repository Registrations
            // ----------------------
            builder.Services.AddScoped<IHostService, HostService>();

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

                policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
                
            });



            builder.Services.AddScoped<UnitOfWork.IUnitOfWork, UnitOfWork.UnitOfWork>();
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

            //builder.Services.AddOpenApi();

            builder.Services.AddOpenApi();
            builder.Services.AddScoped<IFavouriteRepo, FavouriteRepo>();
            builder.Services.AddScoped<IAvailabilityRepo, AvailabilityRepo>();
            builder.Services.AddScoped<IViolationRepo, ViolationRepo>();

            builder.Services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IBookingPaymentService, BookingPaymentService>();
            builder.Services.AddScoped<IBookingPayoutService, BookingPayoutService>();
            builder.Services.AddScoped<IHostPayoutService,HostPayoutService>();

            builder.Services.AddScoped<IPropertyRepo, PropertyRepo>();
            builder.Services.AddScoped<IPropertyImageRepo, PropertyImageRepo>();
            builder.Services.AddScoped<ICancellationPolicyRepo, CancellationPolicyRepo>();
            builder.Services.AddScoped<IPropertyService, PropertyService>();
            builder.Services.AddScoped<IPropertyImageService, PropertyImageService>();
            builder.Services.AddScoped<ICancellationPolicyService, CancellationPolicyService>();
            builder.Services.AddScoped<IReviewRepo, ReviewRepo>();
            builder.Services.AddScoped<IPromotionRepository, PromotionRepository>();
            //AOsama
            builder.Services.AddScoped<IMessageRepo, MessageRepo>();
            builder.Services.AddScoped<IMessageService, MessageService>();
            builder.Services.AddScoped<IConversationRepo, ConversationRepo>();
            builder.Services.AddScoped<INotificationRepo, NotificationRepo>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IConversationService, ConversationService>();
            builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

            builder.Services.AddSignalR();

            builder.Services.AddScoped<IUserUsedPromotionService, UserUsedPromotionService>();
          

            //===============Stripe=========================
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];


            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            // AutoMapper
            builder.Services.AddAutoMapper(cfg => cfg.AddProfile<PropertyProfile>());
            builder.Services.AddAutoMapper(cfg => cfg.AddProfile<PropertyImageProfile>());
            builder.Services.AddAutoMapper(cfg => cfg.AddProfile<CancellationPolicyProfile>());


            //swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v2", new OpenApiInfo
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

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI();
                app.MapHub<ChatHub>("/chatHub", options =>
                {
                    options.Transports = HttpTransportType.WebSockets | HttpTransportType.LongPolling;
                });
            } 
            app.UseHttpsRedirection();

               // app.UseSwaggerUI(options =>
               // {
                 //   options.SwaggerEndpoint("/swagger/v1/swagger.json", "bnbClone API v1");
                 //   options.RoutePrefix = "swagger"; 
                //});
               // app.MapOpenApi();
       
              //  app.UseSwaggerUI(option => option.SwaggerEndpoint("/openapi/v1.json", "v1"));
           // }

                app.UseHttpsRedirection();



            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

