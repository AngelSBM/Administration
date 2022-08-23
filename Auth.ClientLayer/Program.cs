using Auth.DataAccessLayer;
using Auth.DataAccessLayer.Abstractions;
using Auth.DataAccessLayer.Abstractions.Repos;

using Auth.DataAccessLayer.Repositories;

using Auth.LogicLayer.Helpers;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using FluentValidation.AspNetCore;
using System.Text.Json.Serialization;
using Administration.DataAccessLayer.Entities;
using Administration.LogicLayer.Abstractions;
using Administration.LogicLayer.Services;
using Administration.DataAccessLayer.Repositories;
using Administration.DataAccessLayer.Abstractions.Repos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddFluentValidation(fv =>
    {        
        fv.RegisterValidatorsFromAssemblyContaining<Program>();
    });


var connectionString = builder.Configuration.GetConnectionString("LocalServer");
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

//REPOS
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IRepository<Address>, Repository<Address>>();
//SERVICES
builder.Services.AddScoped<IClientService, ClientService>();

//UNITS
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "JWT authentication header [bearer {token}]",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,

    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:8080")
                               .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials(); 
                      });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });
    

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
