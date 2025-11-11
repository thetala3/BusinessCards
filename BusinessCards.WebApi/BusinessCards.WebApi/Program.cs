using BusinessCards.Application.Interfaces;
using BusinessCards.Infrastructure.Persistence;
using BusinessCards.Infrastructure.Services;
using BusinessCards.WebApi.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    //c.SupportNonNullableReferenceTypes();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BusinessCards API", Version = "v1" });
});

var conn = builder.Configuration.GetConnectionString("SqlServer");
builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(conn));
builder.Services.AddScoped<IExportService, ExportService>();
builder.Services.AddScoped<IBusinessCardsService, BusinessCardsService>();

// Program.cs
builder.Services.AddCors(o => o.AddPolicy("AllowDev", p =>
    p.WithOrigins("http://localhost:4200")
     .AllowAnyHeader()
     .AllowAnyMethod()));

var app = builder.Build();
app.UseCors("AllowDev");


GlobalExceptionHandler.Configure(app, app.Logger);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
