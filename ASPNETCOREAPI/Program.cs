using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);
//add cors
builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(
            policy=>
            {
                policy.AllowAnyOrigin();
                policy.AllowAnyMethod();
                policy.AllowAnyHeader();
            }
            );
    }
);
// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(options
    => options.SerializerSettings.ReferenceLoopHandling
    = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("ASPNETCORE_API");
ASPNETCOREAPI.Entities.AspnetcoreApiContext.connectionString = connectionString;
builder.Services.AddDbContext<ASPNETCOREAPI.Entities.AspnetcoreApiContext>(
    options => options.UseSqlServer(connectionString));
builder.Services.AddSingleton<IAuthorizationHandler, ASPNETCOREAPI.Handlers.ValidBirthdayHandle>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Auth", policy => policy.RequireAuthenticatedUser());
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("Staff", policy => policy.RequireRole("Staff"));

    options.AddPolicy("Dev", policy => policy.RequireClaim("IT"));
    options.AddPolicy("CheckYear", policy => policy.AddRequirements(new ASPNETCOREAPI.Requirements.YearOldRequirement(
        Convert.ToInt32(builder.Configuration["ValidYearOld:Min"]),
        Convert.ToInt32(builder.Configuration["ValidYearOld:Max"])
        )));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();



app.UseHttpsRedirection();



//app.UseAuthentication();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();

