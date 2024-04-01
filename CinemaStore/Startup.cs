using CinemaData;
using CinemaStore.Business.Authentication;
using CinemaStore.Business.Halls;
using CinemaStore.Business.Movies;
using CinemaStore.Business.Reservations;
using CinemaStore.Business.Users;
using CinemaStore.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
         *  We add our own services:
         *  
         *  [1] Settings for Communicating TMDB API
         *  [2] ORM (CinemaData/CinemaContext.cs)
         *  [3] The services for Business Logic (CinemaStore/Business/IUserService.cs, UserService.cs etc.)
         *  [4] The mapper for converting from data to store and vice versa (CinemaStore/CinemaStoreProfile.cs)
         *  [5] CORS for client to server communication https://learn.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-6.0
         *  [6] Authentication with JWT Bearer https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity-api-authorization?view=aspnetcore-6.0
         *  [7] Admin Login (Setup.cs)
         *  [8] Tmdb API (CinemaStore/Services/TmdbService.cs)
         *  [9] Controllers (CinemaStore/Controllers)
         *  [10] Swagger Authorization UI (https://learn.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-6.0)
         */
        public void ConfigureServices(IServiceCollection services)
        {
            /****** [1] TMDB API Settings ******/
            var tmdbSettings = configRoot.GetSection("TmdbSettings");
            var apiKey = tmdbSettings.GetValue<string>("ApiKey");

            services.AddHttpClient<TmdbService>(client =>
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            });
            services.AddSingleton(sp => new TmdbService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("tmdb"), apiKey));

            services.AddMvc();

            /****** [2] ORM ******/
            services.AddDbContext<CinemaContext>();

            /****** [3] Services & BL ******/
            services.AddScoped(
                typeof(IAuthenticationService), typeof(AuthenticationService));
            services.AddScoped(
                typeof(IUserService), typeof(UserService));
            services.AddScoped(
                typeof(IMovieService), typeof(MovieService));
            services.AddScoped(
                typeof(IHallService), typeof(HallService));
            services.AddScoped(
                typeof(IReservationService), typeof(ReservationService));
            
            services.AddHttpContextAccessor();

            /****** [4] AutoMapper ******/
            services.AddAutoMapper(typeof(CinemaStoreProfile));
            
            /****** [5] CORS ******/
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

            /****** [6] Authentication with JWT Bearer ******/
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            
            /****** [7] Admin Login ******/
            Setup.configRoot = configRoot;
            Setup.AdminLogin();

            /****** [8] Tmdb API ******/
            services.Configure<TmdbSettings>(configRoot.GetSection("TmdbSettings"));
            services.AddControllersWithViews();

            /****** [9] Controllers ******/
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            
            /****** [10] Swagger Authorization UI ******/
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
