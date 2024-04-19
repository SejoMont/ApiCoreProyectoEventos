using ApiCoreProyectoEventos.Helpers;
using ApiCoreProyectoEventos.Models;
using ApiCoreProyectoEventos.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<HelperPathProvider>();
builder.Services.AddTransient<HelperCryptography>();
builder.Services.AddTransient<HelperMails>();
builder.Services.AddTransient<HelperTools>();

builder.Services.AddTransient<EventosRepository>();


HelperActionServicesOAuth helper =
    new HelperActionServicesOAuth(builder.Configuration);

builder.Services.AddSingleton<HelperActionServicesOAuth>(helper);

builder.Services.AddAuthentication
    (helper.GetAuthenticateSchema())
    .AddJwtBearer(helper.GetJwtBearerOptions());


string connectionString =
    builder.Configuration.GetConnectionString("SqlAzure");

builder.Services.AddDbContext<EventosContext>
    (options => options.UseSqlServer(connectionString));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Api Eventos Sejo",
        Description = "Api Eventos para proyecto de Azure"
    });
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(url: "/swagger/v1/swagger.json"
        , name: "Api Eventos Sejo");
    options.RoutePrefix = "";
});
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{

//}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();