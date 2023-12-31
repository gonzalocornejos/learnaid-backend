using learnaid_backend.Configuration;
using learnaid_backend.Core.Integrations.iText.Interfaces;
using learnaid_backend.Core.Integrations.iText;
using learnaid_backend.Core.Integrations.OpenAI;
using learnaid_backend.Core.Integrations.OpenAI.Interfaces;
using learnaid_backend.Core.Middleware;
using learnaid_backend.Core.Repository;
using learnaid_backend.Core.Repository.Interfaces;
using learnaid_backend.Core.Services;
using learnaid_backend.Core.Services.Interfaces;
using learnaid_backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using learnaid_backend.Core.Integrations.CloudinaryAPI.Interfaces;
using learnaid_backend.Core.Integrations.CloudinaryAPI;
using learnaid_backend.Core.Integrations.GoogleCloud.Interfaces;
using learnaid_backend.Core.Integrations.GoogleCloud;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.Configure<OpenAIConfiguration>(builder.Configuration.GetSection("OpenAI"));
builder.Services.Configure<CloudinaryConfiguration>(builder.Configuration.GetSection("Cloudinary"));
builder.Services.AddControllers();


var services = builder.Services;

services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
); ;
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "LearnAid API", Version = "v1" });

    // Set the comments path for the Swagger JSON and UI.
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

services.AddCors(options =>
{
    options.AddPolicy("Cors", builder =>
    {
        builder
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// Singletons for Injection Dependencies
// Services
services.AddScoped<IUsuarioService, UsuarioService>();
services.AddScoped<IEjercicioService, EjercicioService>();
services.AddScoped<IChatGPTService, ChatGPTService>();
services.AddScoped<IMobileService, MobileService>();

// Repositories
services.AddScoped<IGenericRepository, GenericRepository>();
services.AddScoped<IUsuarioRepository, UsuarioRepository>();
services.AddScoped<IAdaptadoRepository, AdaptadoRepository>();
services.AddScoped<IEjercitacionAdaptadaRepository, EjercitacionAdaptadaRepository>();


// Integrations
services.AddScoped<IOpenAIAPI, OpenAIAPI>();
services.AddScoped<IiText, IText>();
services.AddScoped<ICloudinaryAPI, CloudinaryAPI>();
services.AddScoped<IGoogleCloudAPI, GoogleCloudAPI>();


// AutoMapper

var app = builder.Build();
var environment = app.Environment;
// Configure the HTTP request pipeline.
if (environment.IsDevelopment() || environment.IsEnvironment("Local"))
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "LearnAid API V1"); });
}
else if (environment.IsProduction())
{
    app.UseHttpsRedirection();
}
else
{
    throw new Exception("Invalid environment");
}

app.UseMiddleware<ExceptionHandler>();


app.UseAuthorization();

app.MapControllers();

app.UseCors("Cors");

app.Run();