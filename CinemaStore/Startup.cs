using CinemaData;
using CinemaStore.Business;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CinemaStore
{
    public class Startup
    {
        public IConfiguration configRoot { get; }
        public string MyAllowSpecificOrigins { get; }

        public Startup(IConfiguration configuration)
        {
            configRoot = configuration;       
            MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        }


        /*
         *  Προσθέτουμε τις δικές μας υπηρεσίες:
         *  
         *  [1] Το ORM (CinemaData/CinemaContext.cs)
         *  [2] Τις υπηρεσίες για το Business Logic(CinemaStore/Business/IUserService.cs, UserService.cs)
         *  [3] Τον mapper για την μετατροπή από data σε store και αντίστροφα (CinemaStore/CinemaStoreProfile.cs)
         *  [4] CORS για την επικοινωνία client με server https://learn.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-6.0
         */
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            /****** [1] DatabaseContext ******/
            services.AddDbContext<CinemaContext>();
            /****** [2] Services & BL ******/
            services.AddScoped(
                typeof(IAuthenticationService), typeof(AuthenticationService));
            services.AddScoped(
                typeof(IUserService), typeof(UserService));
            /****** [3] AutoMapper ******/
            services.AddAutoMapper(typeof(CinemaStoreProfile));
            /****** [4] CORS ******/
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                                    policy =>
                                    {
                                        policy.WithOrigins("https://localhost:7236",  // Server's URL
                                                           "http://localhost:3000")   // Client's URL
                                                           .AllowAnyHeader()
                                                           .AllowAnyMethod();
                                    });
            });

            /****** [6] Authentication ******/
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            /****** [7] Jwt Bearer ******/
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configRoot["JWT:Audience"],
                    ValidIssuer = configRoot["JWT:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configRoot["JWT:Key"]))
                };
            });
            /****** [8] Admin Login ******/
            Setup.configRoot = configRoot;
            Setup.AdminLogin();

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            /****** [9] Swagger Authorization UI ******/
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CinemaStore", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Insert the token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
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
                            }
                        },
                        new string[] { }
                    }
                });
            });
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CinemaStore v1");
                });
            }

            app.UseCors(MyAllowSpecificOrigins);

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
