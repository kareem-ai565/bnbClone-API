using bnbClone_API.Data;
using bnbClone_API.Data;
using bnbClone_API.Helpers.MappingProfiles;
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
using bnbClone_API.StripeConfig;
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
using System;
using System.Security.Claims;
using System.Text;
using System.Text;
using TokenService = bnbClone_API.Services.Impelementations.TokenService;
using bnbClone_API.Helpers.MappingProfiles;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Google;



namespace bnbClone_API
{
    public class Program
    {
        public static async Task Main(string[] args)
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
        o.Password.RequiredLength = 0;
        o.Password.RequireNonAlphanumeric = false;
        o.Password.RequireUppercase = false;
        o.Password.RequireLowercase = false;
        o.Password.RequireDigit = false;
        o.Password.RequiredUniqueChars = 0;


        //o.SignIn.RequireConfirmedEmail = true;
    })

    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
       

            builder.Services.AddScoped<UserManager<ApplicationUser>>();
            builder.Services.AddScoped<SignInManager<ApplicationUser>>();
            builder.Services.AddScoped<RoleManager<IdentityRole<int>>>();

            builder.Services.AddScoped<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();

            //// JWT Authentication with enhanced debugging
            //builder.Services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //    .AddJwtBearer(options =>
            //    {
            //        options.SaveToken = true;
            //        options.RequireHttpsMetadata = false;
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuerSigningKey = true,
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"])),
            //            ValidateIssuer = true,
            //            ValidIssuer = builder.Configuration["JWT:Issuer"],
            //            ValidateAudience = true,
            //            ValidAudience = builder.Configuration["JWT:Audience"],
            //            ValidateLifetime = true,
            //            ClockSkew = TimeSpan.Zero,
            //            NameClaimType = ClaimTypes.NameIdentifier,
            //            RoleClaimType = ClaimTypes.Role
            //        };

            //        // Add event logging for debugging with Console.WriteLine
            //        options.Events = new JwtBearerEvents
            //        {
            //            OnAuthenticationFailed = context =>
            //            {
            //                Console.WriteLine($"[DEBUG] JWT Authentication failed: {context.Exception.Message}");
            //                if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            //                {
            //                    context.Response.Headers.Add("Token-Expired", "true");
            //                }
            //                return Task.CompletedTask;
            //            },
            //            OnTokenValidated = context =>
            //            {
            //                Console.WriteLine("[DEBUG] JWT Token validated successfully");
            //                var userIdClaim = context.Principal.FindFirst("UserID")?.Value;
            //                Console.WriteLine($"[DEBUG] UserID from token: {userIdClaim}");
            //                return Task.CompletedTask;
            //            },
            //            OnMessageReceived = context =>
            //            {
            //                Console.WriteLine("[DEBUG] JWT Token received");
            //                var token = context.HttpContext.Request.Cookies["access_token"];
            //                if (!string.IsNullOrEmpty(token))
            //                {
            //                    context.Token = token;
            //                    Console.WriteLine("[DEBUG] Token found in cookie.");
            //                }
            //                return Task.CompletedTask;
            //            },
            //            OnChallenge = context =>
            //            {
            //                Console.WriteLine($"[DEBUG] JWT Challenge: {context.Error}, {context.ErrorDescription}");
            //                return Task.CompletedTask;
            //            }
            //        };
            //    });
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"])),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        NameClaimType = ClaimTypes.NameIdentifier,
        RoleClaimType = ClaimTypes.Role
    };

                    // Add event logging for debugging with Console.WriteLine
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine($"[DEBUG] JWT Authentication failed: {context.Exception.Message}");
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            Console.WriteLine("[DEBUG] JWT Token validated successfully");
                            var userIdClaim = context.Principal.FindFirst("UserID")?.Value;
                            Console.WriteLine($"[DEBUG] UserID from token: {userIdClaim}");
                            return Task.CompletedTask;
                        },
                        OnMessageReceived = context =>
                        {
                            Console.WriteLine("[DEBUG] JWT Token received");
                            var token = context.HttpContext.Request.Cookies["access_token"];
                            if (!string.IsNullOrEmpty(token))
                            {
                                context.Token = token;
                                Console.WriteLine("[DEBUG] Token found in cookie.");
                                return Task.CompletedTask;
                            }

                            // Then check SignalR WebSocket query string
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;

                            if (!string.IsNullOrEmpty(accessToken) &&
                                path.StartsWithSegments("/chatHub"))
                            {
                                context.Token = accessToken;
                                Console.WriteLine("[DEBUG] Token found in SignalR query string.");
                            }

                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            Console.WriteLine($"[DEBUG] JWT Challenge: {context.Error}, {context.ErrorDescription}");
                            return Task.CompletedTask;
                        }
                    };
    // Your existing event logging
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"[DEBUG] JWT Authentication failed: {context.Exception.Message}");
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-Expired", "true");
            }
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("[DEBUG] JWT Token validated successfully");
            var userIdClaim = context.Principal.FindFirst("UserID")?.Value;
            Console.WriteLine($"[DEBUG] UserID from token: {userIdClaim}");
            return Task.CompletedTask;
        },
        //OnMessageReceived = context =>
        //{
        //    Console.WriteLine("[DEBUG] JWT Token received");
        //    var token = context.HttpContext.Request.Cookies["access_token"];
        //    if (!string.IsNullOrEmpty(token))
        //    {
        //        context.Token = token;
        //        Console.WriteLine("[DEBUG] Token found in cookie.");
        //    }
        //    return Task.CompletedTask;
        //},

        OnMessageReceived = context =>
        {
            Console.WriteLine("[DEBUG] JWT Token received - checking multiple sources");

            // Method 1: Check Authorization header first (for localStorage approach)
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                context.Token = authHeader.Substring(7);
                Console.WriteLine("[DEBUG] Token found in Authorization header (localStorage method).");
                return Task.CompletedTask;
            }

            // Method 2: Check cookies (existing cookie approach)
            var cookieToken = context.HttpContext.Request.Cookies["access_token"];
            if (!string.IsNullOrEmpty(cookieToken))
            {
                context.Token = cookieToken;
                Console.WriteLine("[DEBUG] Token found in cookie.");
                return Task.CompletedTask;
            }

            // Method 3: Check query string (useful for SignalR connections)
            var queryToken = context.Request.Query["access_token"].FirstOrDefault();
            if (!string.IsNullOrEmpty(queryToken))
            {
                context.Token = queryToken;
                Console.WriteLine("[DEBUG] Token found in query string.");
                return Task.CompletedTask;
            }

            Console.WriteLine("[DEBUG] No token found in any source (Authorization header, cookies, or query string).");
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            Console.WriteLine($"[DEBUG] JWT Challenge: {context.Error}, {context.ErrorDescription}");
            return Task.CompletedTask;
        }
    };
})
// Google Authentication
.AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["GoogleAuth:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["GoogleAuth:ClientSecret"];
    googleOptions.CallbackPath = "/api/auth/google-callback";

    //  scopes if you need additional user information
    googleOptions.Scope.Add("profile");
    googleOptions.Scope.Add("email");

    // events for debugging
    googleOptions.Events.OnCreatingTicket = context =>
    {
        Console.WriteLine($"[DEBUG] Google authentication successful for: {context.Principal?.FindFirst(ClaimTypes.Email)?.Value}");
        return Task.CompletedTask;
    };

    googleOptions.Events.OnRemoteFailure = context =>
    {
        Console.WriteLine($"[DEBUG] Google authentication failed: {context.Failure?.Message}");
        context.Response.Redirect("/login?error=google_auth_failed");
        context.HandleResponse();
        return Task.CompletedTask;
    };
});

            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
            });
// solved conflifct
//                 .AddJwtBearer(options =>
//                 {
//                     options.SaveToken = true;
//                     options.RequireHttpsMetadata = false;
//                     options.TokenValidationParameters = new TokenValidationParameters
//                     {
//                         ValidateIssuerSigningKey = true,
//                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"])),
//                         ValidateIssuer = true,
//                         ValidIssuer = builder.Configuration["JWT:Issuer"],
//                         ValidateAudience = true,
//                         ValidAudience = builder.Configuration["JWT:Audience"],
//                         ValidateLifetime = true,
//                         ClockSkew = TimeSpan.Zero,
//                         NameClaimType = ClaimTypes.NameIdentifier,
//                         RoleClaimType = ClaimTypes.Role
//                     };

//                     // Add event logging for debugging with Console.WriteLine
//                     options.Events = new JwtBearerEvents
//                     {
//                         OnAuthenticationFailed = context =>
//                         {
//                             Console.WriteLine($"[DEBUG] JWT Authentication failed: {context.Exception.Message}");
//                             if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
//                             {
//                                 context.Response.Headers.Add("Token-Expired", "true");
//                             }
//                             return Task.CompletedTask;
//                         },
//                         OnTokenValidated = context =>
//                         {
//                             Console.WriteLine("[DEBUG] JWT Token validated successfully");
//                             var userIdClaim = context.Principal.FindFirst("UserID")?.Value;
//                             Console.WriteLine($"[DEBUG] UserID from token: {userIdClaim}");
//                             return Task.CompletedTask;
//                         },
//                         OnMessageReceived = context =>
//                         {
//                             Console.WriteLine("[DEBUG] JWT Token received");
//                             return Task.CompletedTask;
//                         },
//                         OnChallenge = context =>
//                         {
//                             Console.WriteLine($"[DEBUG] JWT Challenge: {context.Error}, {context.ErrorDescription}");
//                             return Task.CompletedTask;
//                         }
//                     };
//                 });


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
            builder.Services.AddScoped<IEmailService, EmailService>();

      
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
                options.AddPolicy("DevelopmentCorsPolicy", policy =>
                {
                    policy.WithOrigins("http://localhost:4200", "https://localhost:4200" , "http://airlacasa.runasp.net") // Your Angular app URL
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials() // Required for cookies
                          .SetIsOriginAllowed(_ => true)
                          .SetPreflightMaxAge(TimeSpan.FromMinutes(10));

                });
            });


            builder.Services.AddScoped<UnitOfWork.IUnitOfWork, UnitOfWork.UnitOfWork>();
            builder.Services.AddScoped<IPropertyAmenityService, PropertyAmenityService>();
            builder.Services.AddScoped<IAmenityService, AmenityService>();
            builder.Services.AddScoped<IPropertyCategoryService, PropertyCategoryService>();
            builder.Services.AddScoped<IhostVerificationService, hostVerificationService>();

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals;
                }); ;
            builder.Services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            });


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
            builder.Services.AddScoped<IHostPayoutService, HostPayoutService>();

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




            builder.Services.Configure<FormOptions>(o =>
            {
                o.MultipartBodyLengthLimit = 10 * 1024 * 1024; // 10 MB
            });

            //===============Stripe=========================
            builder.Services.Configure<StripeConfig.Stripe>(builder.Configuration.GetSection("Stripe"));

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



            app.UseCors("AllowAngularApp");
            // ----------------------
            // Seed Roles
            // ----------------------
            try
            {
                using (var scope = app.Services.CreateScope())
                {
                    await SeedRolesAsync(scope.ServiceProvider);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to seed roles: {ex.Message}");
            }

            //app.UseCors("AllowAll");
            app.UseCors("DevelopmentCorsPolicy");
            //app.UseCors("StrictPolicy");

            app.UseStaticFiles(); // ⬅️ مهم جدًا لعرض الصور من wwwroot



            // Configure the HTTP request pipeline.

            app.UseStaticFiles();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.MapHub<ChatHub>("/chatHub", options =>
                {
                    options.Transports = HttpTransportType.WebSockets | HttpTransportType.LongPolling;
                }).RequireCors("DevelopmentCorsPolicy");
            }

            app.UseHttpsRedirection();

            // Debug middleware for JWT debugging - ADD THIS BEFORE UseAuthentication
            app.Use(async (context, next) =>
            {
                if (context.Request.Headers.ContainsKey("Authorization"))
                {
                    var authHeader = context.Request.Headers["Authorization"].ToString();
                    Console.WriteLine($"[DEBUG] Authorization header: {authHeader}");

                    if (authHeader.StartsWith("Bearer "))
                    {
                        var token = authHeader.Substring(7);
                        Console.WriteLine($"[DEBUG] Extracted token: {token.Substring(0, Math.Min(50, token.Length))}...");
                    }
                    else
                    {
                        Console.WriteLine("[DEBUG] Authorization header doesn't start with 'Bearer '");
                    }
                }
                else
                {
                    Console.WriteLine("[DEBUG] No Authorization header found");
                }


                await next();

                // Check authentication result
                Console.WriteLine($"[DEBUG] After authentication - IsAuthenticated: {context.User?.Identity?.IsAuthenticated ?? false}, Identity: {context.User?.Identity?.Name ?? "null"}");
            });

            app.UseAuthentication();
            app.UseAuthorization();

            //app.MapControllers();
            app.MapControllers().RequireCors("DevelopmentCorsPolicy");

            app.Run();
        }

        // Method to seed roles
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

            string[] roleNames = { UserRoleConstants.Guest, UserRoleConstants.Host, UserRoleConstants.Admin };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole<int>(roleName));
                    Console.WriteLine($"[DEBUG] Created role: {roleName}");
                }
                else
                {
                    Console.WriteLine($"[DEBUG] Role already exists: {roleName}");
                }

            }
        }

    }
}

