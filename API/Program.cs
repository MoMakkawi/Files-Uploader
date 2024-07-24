using API;

using Hangfire;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder
    .AddConfigurations()
    .AddServices();

var app = builder.Build();

app.UseAuthentication()
    .UseAuthorization();

app.UseHangfireDashboard()
    .UseHangfireServer();

app.MapControllers();

app.Run();

Setup.AddBackgroundServices();