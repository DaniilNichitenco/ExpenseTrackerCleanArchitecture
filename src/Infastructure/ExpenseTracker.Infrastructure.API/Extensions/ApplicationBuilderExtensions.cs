using ExpenseTracker.Infrastructure.Repository.API.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace ExpenseTracker.Infrastructure.Repository.API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder AddExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandler>();
        }
    }
}
