using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NZWalks.api.Data;
using NZWalks.api.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// this is for fluent validation
builder.Services.AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<Program>());

builder.Services.AddDbContext<NZWalksDbContext>(options =>{

    options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalks"));

});

// here I will add a new service of Interface throuh interface class
// it means if I call IRegionRepository interface, it will give the implemnetaion of RegionRepository

builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<IWalksRepository, WalksRepository>();
builder.Services.AddScoped<IWalkDifficultyRepository, WalkDifficultyRepository>();
builder.Services.AddScoped<ITokenHandlerRepository, TokenHandlerRepository>();

// for using static Reposiorty I will use singleton
//And with that we have injected IUserRepository a repository to be used inside anywhere.

builder.Services.AddSingleton<IUserRepository, StaticUserRepository>();

//Here We have injected automapper. when application start it will call the assembly my automapper for using all Profiles class
builder.Services.AddAutoMapper(typeof(Program).Assembly);


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))


    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// these are the options of middle point pipe line
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
