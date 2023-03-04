using api.Contracts;
using api.Models;
using api.Persistance;
using api.Repository;
using api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddScoped<IOpenData, OpenData>();
    builder.Services.AddScoped<ICarRepository, CarRepository>();
    builder.Services.AddDbContext<Datacontext>(opt => opt.UseInMemoryDatabase("Database"));
    builder.Services.Configure<OpenDataSettings>(builder.Configuration.GetSection(nameof(OpenDataSettings)));
}


var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
