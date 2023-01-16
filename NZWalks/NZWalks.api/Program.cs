using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NZWalks.api.Data;
using NZWalks.api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<NZWalksDbContext>(options =>{

    options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalks"));

});

// here I will add a new service of Interface throuh interface class
// it means if I call IRegionRepository interface, it will give the implemnetaion of RegionRepository

builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<IWalksRepository, WalksRepository>();
builder.Services.AddScoped<IWalkDifficultyRepository, WalkDifficultyRepository>();

//Here We have injected automapper. when application start it will call the assembly my automapper for using all Profiles class
builder.Services.AddAutoMapper(typeof(Program).Assembly);




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
