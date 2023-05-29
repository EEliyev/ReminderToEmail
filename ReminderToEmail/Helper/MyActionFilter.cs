using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace ReminderToEmail.Helper
{
    public class MyActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var req= context.HttpContext.Request;

            var authorization = context.HttpContext.Request.Headers["Authorization"];

            if (AuthenticationHeaderValue.TryParse(authorization,out var token))
            {
                try
                {
                    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

                    // Token'i çözümle
                    JwtSecurityToken jwt = tokenHandler.ReadJwtToken(token.Parameter);

                    // Claims'leri al
                    var claims = jwt.Claims.ToArray();

                    context.HttpContext.Request.Headers["Id"] = claims[0].Value;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
               
            }
            // our code before action executes
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // our code after action executes
        }
    }
}
