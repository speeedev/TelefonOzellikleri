using Microsoft.EntityFrameworkCore;
using TelefonOzellikleri.Data;

namespace TelefonOzellikleri.Middleware
{
    public class MaintenanceMiddleware
    {
        private readonly RequestDelegate _next;

        public MaintenanceMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value ?? "";

            if (path.StartsWith("/derin", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("/lib/", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("/css/", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("/js/", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("/favicon", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            var dbContext = context.RequestServices.GetRequiredService<TelefonOzellikleriDbContext>();
            var settings = await dbContext.SiteSettings.AsNoTracking().FirstOrDefaultAsync();

            if (settings?.IsMaintenanceMode == true)
            {
                context.Response.StatusCode = 503;
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.WriteAsync(MaintenancePage());
                return;
            }

            await _next(context);
        }

        private static string MaintenancePage()
        {
            return """
            <!DOCTYPE html>
            <html lang="tr">
            <head>
                <meta charset="utf-8" />
                <meta name="viewport" content="width=device-width, initial-scale=1.0" />
                <title>Bakım Modu</title>
                <style>
                    * { margin: 0; padding: 0; box-sizing: border-box; }
                    body {
                        font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
                        background: #f0f2f5;
                        display: flex;
                        align-items: center;
                        justify-content: center;
                        min-height: 100vh;
                        color: #333;
                    }
                    .container {
                        text-align: center;
                        max-width: 500px;
                        padding: 2rem;
                    }
                    .icon {
                        font-size: 4rem;
                        margin-bottom: 1.5rem;
                    }
                    h1 {
                        font-size: 1.8rem;
                        font-weight: 700;
                        color: #1a1a2e;
                        margin-bottom: 0.75rem;
                    }
                    p {
                        font-size: 1.05rem;
                        color: #666;
                        line-height: 1.6;
                    }
                </style>
            </head>
            <body>
                <div class="container">
                    <div class="icon">&#128736;</div>
                    <h1>Şu anda çok yoğunuz</h1>
                    <p>Sitemiz şu anda bakım modundadır. Lütfen daha sonra tekrar deneyiniz.</p>
                </div>
            </body>
            </html>
            """;
        }
    }
}
