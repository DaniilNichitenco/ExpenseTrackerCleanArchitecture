using ExpenseTracker.Core.Domain.Exceptions;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ExpenseTracker.Infrastructure.Repository.API.Middlewares
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly TelemetryClient _telemetryClient;
        public ExceptionHandler(RequestDelegate next, TelemetryClient telemetryClient)
        {
            _next = next;
            _telemetryClient = telemetryClient;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (EntityNotFoundException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound);
            }
            catch (EntityExistsException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);
            }
            catch (BadRequestException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode)
        {
            _telemetryClient.TrackException(exception);

            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)statusCode;
            await response.WriteAsync(JsonConvert.SerializeObject(new
            {
                message = exception.Message
            }));
        }
    }
}
