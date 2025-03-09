using Microsoft.EntityFrameworkCore;
using NorthwindRestApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Dependency Injektiolla välitetty tietokantatieto kontrollereille (Rekisteröi NorthwindContext ja liitä yhteysmerkkijono appsettings.json:ista)
builder.Services.AddDbContext<NorthwindContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("Paikallinen")));

//-----Cros määritys------- (CORS controls who can access your API)
builder.Services.AddCors(options =>
{
    options.AddPolicy("all",
        builder => builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("all");    // // Apply the CORS policy named "all"

app.UseAuthorization();

app.MapControllers();

app.Run();
