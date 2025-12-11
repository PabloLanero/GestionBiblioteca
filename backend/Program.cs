using System.Reflection;
using Biblio.Controllers;
using Biblio.Repositories;
using Biblio.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<ILibroRepository,LibroRepository>();
builder.Services.AddScoped<IAutorRepository,AutorRepository>();
builder.Services.AddScoped<IEditorialRepository,EditorialRepository>();
builder.Services.AddScoped<IUsuarioRepository,UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService,UsuarioService>();
builder.Services.AddScoped<IAutorService,AutorService>();
builder.Services.AddScoped<ILibroService,LibroService>();
builder.Services.AddScoped<IEditorialService,EditorialService>();
builder.Services.AddScoped<IPrestamoRepository,PrestamoRepository>();
builder.Services.AddScoped<IPrestamoService,PrestamoService>();
builder.Services.AddScoped<IResenaRepository,ResenaRepository>();
builder.Services.AddScoped<IResenaService,ResenaService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API de la biblioteca",
        Description = "AVISO: NINGUN ID ES AUTO INCREMENTAL, PUEDE PROVOCAR MUCHOS ERRORES",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
//Para el cors
string  MyAllowSpecificOrigins = "MiPoliticaDejaATodos";
builder.Services.AddCors(options =>
{
    //Se puede configuara para que unos pocos entren, pero aqui no es el caso
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy  =>
                      {
                          policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                      });
});
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();



app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
