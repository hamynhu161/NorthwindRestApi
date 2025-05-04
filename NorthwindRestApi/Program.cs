using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NorthwindRestApi.Models;
using NorthwindRestApi.Services;
using NorthwindRestApi.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//-----Cros m‰‰ritys------- (CORS controls who can access your API)
builder.Services.AddCors(options =>
{
    options.AddPolicy("all",
        builder => builder.WithOrigins(
                "http://localhost:3000",
                "http://localhost:5174")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());                       //allow the frontend JavaScript code to access the response body of a cross-origin request when authentication is involved
});

//Dependency Injektiolla v‰litetty tietokantatieto kontrollereille (Rekisterˆi NorthwindContext ja liit‰ yhteysmerkkijono appsettings.json:ista)
builder.Services.AddDbContext<NorthwindContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("Pilvi")));


// ----- tuodaan appSettings.jsoniin tekem‰mme AppSettings m‰‰ritys ------

var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);


//------JWT Autentikaatio----------
var appSettings = appSettingsSection.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSettings.Key);

builder.Services.AddAuthentication(au =>
{
    au.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    au.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwt =>
{
    jwt.RequireHttpsMetadata = false;
    jwt.SaveToken = true;
    jwt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddScoped<IAuthenticateService, AuthenticateService>();

//-------JWT m‰‰ritys p‰‰ttyy-------------



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("all");    // // Apply the CORS policy named "all"

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
