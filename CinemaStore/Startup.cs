using CinemaData;
using CinemaStore.Business;

namespace CinemaStore
{
    public class Startup
    {
        public IConfiguration configRoot
        {
            get;
        }

        public string MyAllowSpecificOrigins { get; };

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
            /****** [1] ******/
            services.AddDbContext<CinemaContext>();
            /****** [2] ******/
            services.AddScoped(
                typeof(IUserService), typeof(UserService));
            /****** [3] ******/
            services.AddAutoMapper(typeof(CinemaStoreProfile));
            /****** [4] ******/
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


            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(MyAllowSpecificOrigins);

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
