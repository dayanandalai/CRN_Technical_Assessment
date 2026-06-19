namespace CRN_Technical_Assessment.api.Middleware
{
    /// <summary>
    /// Middleware to add security headers to all HTTP responses
    /// Protects against common web vulnerabilities
    /// </summary>
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Prevent MIME sniffing - forces browser to respect Content-Type
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";

            // Prevent clickjacking attacks
            context.Response.Headers["X-Frame-Options"] = "DENY";

            // Enable XSS protection in older browsers
            context.Response.Headers["X-XSS-Protection"] = "1; mode=block";

            // Strict Transport Security - enforces HTTPS
            // max-age: 1 year (31536000 seconds), includes subdomains, preload
            context.Response.Headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains; preload";

            // Content Security Policy - restricts resource loading
            context.Response.Headers["Content-Security-Policy"] = "default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline'; img-src 'self' data: https:;";

            // Referrer Policy - controls what referrer info is sent
            context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";

            // Feature Policy / Permissions Policy
            context.Response.Headers["Permissions-Policy"] = "geolocation=(), microphone=(), camera=()";

            await _next(context);
        }
    }
}
