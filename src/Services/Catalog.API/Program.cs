using BuildingBlocks.Exceptions.Handler;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.AddBehavior(typeof(IPipelineBehavior<,>),typeof(ValidationBehavior<,>));
    config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
}
);

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("CatalogDb")!);
}).UseLightweightSessions();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("CatalogDb")!);
var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapCarter();

app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.UseExceptionHandler("/error");

app.Run();
