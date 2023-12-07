using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WukkaManCleaning_API.Data;
using WukkaManCleaning_API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//----------------------------------------------------------

// Dipendency Injection for ApplicationDbContext
builder.Services.AddDbContext<IdentityApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("JevConnection")));


//-------------------------------------------------New Things ---------------------------------------------


//Add Identity User // Store
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 5;
}).AddEntityFrameworkStores<IdentityApplicationDbContext>().AddDefaultTokenProviders();

//add
builder.Services.AddTransient<IAuthService, AuthService>();


//Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin",policy => policy.Requirements.Add(new AdminAcess()));
});
//------------------


//Register AdminAuthHandeler for DI //new stuff
builder.Services.AddSingleton<IAuthorizationHandler, AdminAuthHandeler>();


//Authentication ------------------
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        RequireExpirationTime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetSection("JWTConfig: Issuer").Value,
        ValidAudience = builder.Configuration.GetSection("JWTConfig: Audience").Value,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWTConfig:Key").Value))
    };
});
//---------------------------------------------------------------------------------------------------------------------------------------



//------------------------------


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//Added
app.UseAuthentication();



app.UseHttpsRedirection();


app.UseAuthorization();



app.MapControllers();

app.Run();
