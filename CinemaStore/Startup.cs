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

        public Startup(IConfiguration configuration)
        {
            configRoot = configuration;       
        }


        /*
         *  Προσθέτουμε τις δικές μας υπηρεσίες:
         *  
         *  [1] Το ORM (CinemaData/CinemaContext.cs)
         *  [2] Τις υπηρεσίες για το Business Logic(CinemaStore/Business/IUserService.cs, UserService.cs)
         *  [3] Τον mapper για την μετατροπή από data σε store και αντίστροφα (CinemaStore/CinemaStoreProfile.cs)
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

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
