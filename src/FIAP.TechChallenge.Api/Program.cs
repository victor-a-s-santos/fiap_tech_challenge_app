using FIAP.Crosscutting.Infrastructure.Contexts.SqlServer;
using FIAP.TechChallenge.Api.Configurations;
using FIAP.TechChallenge.Api.Conventions;
using FIAP.TechChallenge.Infrastructure.Contexts;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

IConfiguration configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddDependencyInjectionSetup();
//builder.Services.AddTechChallengeAuthentication();
builder.Services.AddAutoMapperSetup();
builder.Services.AddSwaggerSetup();
builder.Services.AddPostgressContext<SqlContext>(configuration);
builder.Services.AddResponseCompression();
builder.Services.Configure<GzipCompressionProviderOptions>(opt => opt.Level = CompressionLevel.Optimal);
builder.Services.AddCryptographySetup(configuration);
//builder.Services.AddSettingSetup(configuration);
builder.Services.AddControllers(x => x.Conventions.Add(new ControllerDocumentationConvention()))
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddOptions();
builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.AddApiVersioning(v =>
{
    v.AssumeDefaultVersionWhenUnspecified = true;
});

builder.Services.AddTokenCredentialSetup(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MigrationAndSeedDatabase();

app.UseDeveloperExceptionPage();
app.UseSwaggerSetup();
app.UseApiVersioning();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

app.UseCors(c =>
{
    c.AllowAnyHeader();
    c.AllowAnyMethod();
    c.AllowAnyOrigin();
});

app.MapControllers();

app.Run();