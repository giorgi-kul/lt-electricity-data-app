using ElectricityDataApp.Api;
using ElectricityDataApp.Api.Services;
using ElectricityDataApp.Application;
using ElectricityDataApp.Infrastructure;
using ElectricityDataApp.Infrastructure.Persistence;
using Hangfire;
using Hangfire.SqlServer;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();


// Add services to the container.
builder.Services.AddApi(builder.Configuration, builder.Host);
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHangfireDashboard();

    using (var scope = app.Services.CreateScope())
    {
        var initialiser = scope.ServiceProvider.GetRequiredService<DataContextInitializer>();
        await initialiser.InitializeAsync();
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
