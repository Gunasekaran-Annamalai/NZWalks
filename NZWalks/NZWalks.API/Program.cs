using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/*  -> We are adding our DbContext class "NZWalksDbContext" as a dependency
    -> so that whenever the DbContext class is used it will take the connection string and the options from here.
    -> We also passed the options to the base class in DbConext as well
    -> In this method we'll not create an object but pass in the dependencies as parameters to the base class. */
builder.Services.AddDbContext<NZWalksDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksConnectionString")));

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
