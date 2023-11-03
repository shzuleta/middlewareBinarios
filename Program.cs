using FBapiService.DataDB;
using FBapiService.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AspNetCore.Authentication.ApiKey;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BanticfintechContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL")));

builder.Services.AddControllers();
builder.Services.AddScoped<UserDataCrud>();

//builder.Services.AddAuthentication("BasicAuthentication")
//    .AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("BasicAuthentication", null);

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//   .AddJwtBearer(options =>
//{
//    options.RequireHttpsMetadata = false;
//    options.SaveToken = true;
//    options.TokenValidationParameters = new TokenValidationParameters()
//    {
//        ValidateIssuer = true, // Valida el emisor (issuer) del token
//        ValidateAudience = true, // Valida la audiencia (audience) del token
//        ValidateLifetime = true, // Valida el tiempo de vida (expiración) del token
//        ValidateIssuerSigningKey = true, // Valida la clave de firma del token
//        ValidIssuer = builder.Configuration["Jwt:Issuer"], // Reemplaza esto con el emisor válido de tus tokens
//        ValidAudience = builder.Configuration["Jwt:Audience"],  // Reemplaza esto con la audiencia válida de tus tokens
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // Reemplaza esto con tu clave secreta de firma
//    };

//});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("BasicAuthentication", null)
.AddJwtBearer(jwtOptions =>
{
    jwtOptions.RequireHttpsMetadata = false;
    jwtOptions.SaveToken = true;
    jwtOptions.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true, // Valida el emisor (issuer) del token
        ValidateAudience = true, // Valida la audiencia (audience) del token
        ValidateLifetime = true, // Valida el tiempo de vida (expiración) del token
        ValidateIssuerSigningKey = true, // Valida la clave de firma del token
        ValidIssuer = builder.Configuration["Jwt:Issuer"], // Reemplaza esto con el emisor válido de tus tokens
        ValidAudience = builder.Configuration["Jwt:Audience"],  // Reemplaza esto con la audiencia válida de tus tokens
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // Reemplaza esto con tu clave secreta de firma
    };

}); 

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddAuthentication(ApiKeySchemeOptions.Scheme)
//    .AddScheme<ApiKeySchemeOptions, ApiKeySchemeHandler>(
//        ApiKeySchemeOptions.Scheme, options =>
//        {
//            options.HeaderName = "X-API-KEY";
//        });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
