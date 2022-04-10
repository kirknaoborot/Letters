using Letters.Data;
using Letters.Service.Interfaces;
using Letters.Service.Services;
using Letters.Service.Settings;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddDbContext<ReceptionContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("ReceptionConnection")));
builder.Services.Configure<Captcha>(builder.Configuration.GetSection("CaptchaSettings"));
builder.Services.AddTransient<IFormService, FormService>();
builder.Services.AddTransient<ICaptchaService, CaptchaService>();
builder.Services.AddMemoryCache();

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
