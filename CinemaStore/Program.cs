using CinemaData;
using CinemaStore;
using CinemaStore.Business;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/*
 *  Προσθέτουμε τις δικές μας υπηρεσίες:
 *  [1] Το ORM (CinemaData/CinemaContext.cs)
 *  [2] Τις υπηρεσίες (CinemaStore/Business/IUserService.cs, UserService.cs)
 *  [3] Τον mapper για την μετατροπή από data σε store και αντίστροφα (CinemaStore/CinemaStoreProfile.cs)
 */

// [1] Το ORM (CinemaData/CinemaContext.cs)
builder.Services.AddDbContext<CinemaContext>();

// [2] Τις υπηρεσίες (CinemaStore/Business/IUserService.cs, UserService.cs)
builder.Services.AddScoped(
    typeof(IUserService), typeof(UserService));

// [3] Τον mapper για την μετατροπή από data σε store και αντίστροφα (CinemaStore/CinemaStoreProfile.cs)
builder.Services.AddAutoMapper(typeof(CinemaStoreProfile));

/*
 *  Ανάμεσα στα σχόλια με τα αστεράκια προσθέτω τις δικές μου υπηρεσίες.
 */

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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