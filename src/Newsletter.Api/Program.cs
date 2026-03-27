using FastEndpoints;
using Microsoft.OpenApi;


var bld = WebApplication.CreateBuilder();
bld.Services.AddFastEndpoints();

// Swagger / OpenAPI
bld.Services.AddEndpointsApiExplorer();
bld.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Newsletter API",
        Version = "v1",
        Description = "API for subscribing to and querying newsletter information"
    });
});

var app = bld.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Newsletter API v1");
        c.RoutePrefix = string.Empty; // serve Swagger UI at application root (https://localhost:7027/)
    });
}

app.UseFastEndpoints();
app.Run();