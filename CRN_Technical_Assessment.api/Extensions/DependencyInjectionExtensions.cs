using CRN_Technical_Assessment.api.Middleware;
using CRN_Technical_Assessment.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CRN_Technical_Assessment.api.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApiServices(
            this IServiceCollection services)
        {
            // Add validators
            services.AddScoped<IValidator<CRN_Technical_Assessment.Application.DTOs.CreateProductDto>, CreateProductDtoValidator>();
            services.AddScoped<IValidator<CRN_Technical_Assessment.Application.DTOs.UpdateProductDto>, UpdateProductDtoValidator>();
            services.AddScoped<IValidator<CRN_Technical_Assessment.Application.DTOs.CreateCategoryDto>, CreateCategoryDtoValidator>();
            services.AddScoped<IValidator<CRN_Technical_Assessment.Application.DTOs.RegisterUserDto>, RegisterUserDtoValidator>();
            services.AddScoped<IValidator<CRN_Technical_Assessment.Application.DTOs.LoginRequestDto>, LoginRequestDtoValidator>();

            // Add CORS policy - Configure to allow specific origins
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:3000", "https://localhost:3000", "https://yourdomain.com")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithExposedHeaders("Authorization", "X-Pagination");
                });

                // Fallback policy for development
                options.AddPolicy("AllowAll", policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            return services;
        }

        public static IApplicationBuilder UseApiMiddleware(
            this IApplicationBuilder app)
        {
            // Add security headers middleware
            app.UseMiddleware<SecurityHeadersMiddleware>();

            // Use CORS policy
            var environment = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
            if (environment.IsDevelopment())
            {
                app.UseCors("AllowAll");
            }
            else
            {
                app.UseCors("AllowSpecificOrigins");
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            return app;
        }
    }
}
