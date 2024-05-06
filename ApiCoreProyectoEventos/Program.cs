using ApiCoreProyectoEventos.Helpers;
using ApiCoreProyectoEventos.Models;
using ApiCoreProyectoEventos.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NSwag.Generation.Processors.Security;
using NSwag;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Azure;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAzureClients(factory =>
{
    factory.AddSecretClient
    (builder.Configuration.GetSection("KeyVault"));
});

SecretClient secretClient = builder.Services.BuildServiceProvider().GetService<SecretClient>();

KeyVaultSecret connectionStringSecret = await secretClient.GetSecretAsync("connectionstring");
KeyVaultSecret issuerSecret = await secretClient.GetSecretAsync("issuer");
KeyVaultSecret audienceSecret = await secretClient.GetSecretAsync("audience");
KeyVaultSecret secretKeySecret = await secretClient.GetSecretAsync("secretkey");

string connectionString = connectionStringSecret.Value;
string issuer = issuerSecret.Value;
string audience = audienceSecret.Value;
string secretKey = secretKeySecret.Value;

// Add services to the container.
builder.Services.AddTransient<HelperPathProvider>();
builder.Services.AddTransient<HelperCryptography>();
builder.Services.AddTransient<HelperMails>();
builder.Services.AddTransient<HelperTools>();

builder.Services.AddTransient<EventosRepository>();

HelperActionServicesOAuth helper = new HelperActionServicesOAuth(issuer, audience, secretKey);

builder.Services.AddSingleton<HelperActionServicesOAuth>(helper);

builder.Services.AddAuthentication(helper.GetAuthenticateSchema()).AddJwtBearer(helper.GetJwtBearerOptions());

builder.Services.AddDbContext<EventosContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApiDocument(document =>
{
    document.Title = "Api Eventos ExitZ";
    document.Description = "Api Eventos para proyecto de Azure";
    document.AddSecurity("JWT", Enumerable.Empty<string>(),
        new NSwag.OpenApiSecurityScheme
        {
            Type = OpenApiSecuritySchemeType.ApiKey,
            Name = "Authorization",
            In = OpenApiSecurityApiKeyLocation.Header,
            Description = "Copia y pega el Token en el campo 'Value:' as: Bearer {Token JWT}."
        }
    );
    document.OperationProcessors.Add(
    new AspNetCoreOperationSecurityScopeProcessor("JWT"));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseOpenApi();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(url: "/swagger/v1/swagger.json"
        , name: "Api Eventos Sejo");
    options.RoutePrefix = "";
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();