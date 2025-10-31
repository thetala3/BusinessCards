using Microsoft.AspNetCore.Diagnostics;

namespace BusinessCards.WebApi.Middleware
{
    public class GlobalExceptionHandler
    {
        public static void Configure(IApplicationBuilder app, ILogger logger)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";

                    var feature = context.Features.Get<IExceptionHandlerFeature>();
                    if (feature != null)
                    {
                        var ex = feature.Error;
                        logger.LogError(ex, "Unhandled exception occurred.");

                        var result = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            message = "Something went wrong.",
                            error = ex.Message
                        });
                        await context.Response.WriteAsync(result);
                    }
                });
            });
        }
    }
}
