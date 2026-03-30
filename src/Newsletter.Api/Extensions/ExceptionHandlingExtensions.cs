using Microsoft.AspNetCore.Diagnostics;

namespace Newsletter.Api.Extensions;

public static class ExceptionHandlingExtensions
{
    public static WebApplication UseGlobalExceptionHandling(this WebApplication app)
    {
        app.UseExceptionHandler(exceptionApp =>
        {
            exceptionApp.Run(async context =>
            {
                var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                var logger = context.RequestServices.GetRequiredService<ILoggerFactory>()
                    .CreateLogger("GlobalExceptionHandler");

                logger.LogError(exception, "Unhandled exception for request {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path);

                var detail = app.Environment.IsDevelopment() ? exception?.Message : null;

                await Results.Problem(
                    title: "An unexpected error occurred.",
                    detail: detail,
                    statusCode: StatusCodes.Status500InternalServerError).ExecuteAsync(context);
            });
        });

        return app;
    }
}