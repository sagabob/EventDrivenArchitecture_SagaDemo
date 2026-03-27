using FastEndpoints;
using FastEndpoints.Swagger;

var bld = WebApplication.CreateBuilder();

// Register FastEndpoints and configure its Swagger document once
bld.Services.AddFastEndpoints()
    .SwaggerDocument(o =>
    {
        o.DocumentSettings = s =>
        {
            s.Title = "Newsletter API";
            s.Version = "v1";
        };
    });

var app = bld.Build();

// Enable endpoints and FastEndpoints' Swagger middleware
app.UseFastEndpoints()
   .UseSwaggerGen();

app.Run();