using api.Authenticaion;
using api.Models;
using api.Persistance;
using api.Repository;
using api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(x =>
    {
        x.SwaggerDoc("v1",
                 new OpenApiInfo { Title = "Pandora.Api", Version = "v1" });
        var xmlFile = "Pandora.Api.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        x.IncludeXmlComments(xmlPath);
    }
    );
    builder.Services.AddScoped<IOpenDataService, OpenDataService>();
    builder.Services.AddScoped<ICarRepository, CarRepository>();
    builder.Services.AddScoped<ApiKeyAuthFilter>();
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
