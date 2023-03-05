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
        var xmlFile = "api.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        x.IncludeXmlComments(xmlPath);
        x.AddSecurityDefinition("Pandora-Api-Key", new OpenApiSecurityScheme
        {
            Name = "Pandora-Api-Key",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Description = "API key needed to access the endpoints",
            Scheme = "ApiKey"
        });
        x.AddSecurityRequirement
        (
            new OpenApiSecurityRequirement
                {
                {
                    new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Pandora-Api-Key"
                            }
                        },
                        new string[] {}
                }
                }
        );
    }
    );
    builder.Services.AddScoped<IOpenDataService, OpenDataService>();
    builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
    builder.Services.AddScoped<ApiKeyAuthFilter>();
    builder.Services.AddDbContext<Datacontext>(opt => opt.UseInMemoryDatabase("Database"));
    builder.Services.Configure<OpenDataSettings>(builder.Configuration.GetSection(nameof(OpenDataSettings)));
}


var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(x =>
        {
            x.SwaggerEndpoint("/swagger/v1/swagger.json", "api v1");
        });
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
