using Letters.Data;
using Letters.Service;
using Letters.Service.Interfaces;
using Letters.Service.Services;
using Letters.Service.Settings;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseKestrel(so =>
{
    so.Limits.MaxRequestBodySize = 52428800;
});

builder.WebHost.CaptureStartupErrors(true).UseSetting("detailedErrors", "true");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddServices(builder.Configuration);

builder.Services.Configure<Captcha>(options => builder.Configuration.GetSection("CaptchaSettings").Bind(options));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
