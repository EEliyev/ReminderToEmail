using Newtonsoft.Json;
using System.Net;

namespace ReminderToEmail.Helper
{
    public class ErrorHandling
    {
        private readonly RequestDelegate _next;
        public ErrorHandling(RequestDelegate next)
        {
            this._next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {

                await HandleExceptionAsync(context,ex);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context,Exception ex)
        {
            var code = HttpStatusCode.InternalServerError;
            var rs = JsonConvert.SerializeObject(new { error = ex.Message });
            context.Response.ContentType="application/json";
            context.Response.StatusCode=(int)code;
            return context.Response.WriteAsync(rs);
        }
    }
}
