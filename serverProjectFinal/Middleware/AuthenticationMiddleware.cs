using serverProjectFinal.DAL;
using serverProjectFinal.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System.Net.Http;
using serverProjectFinal.DTO;
using System.ComponentModel.DataAnnotations;

namespace serverProjectFinal.Middleware

{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthenticationMiddleware> _logger;

        public AuthenticationMiddleware(RequestDelegate next, ILogger<AuthenticationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
       {
            try
            {
                var identity = context.User.Identity as ClaimsIdentity;
                if (identity == null)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Unauthorized");
                    return;
                }

                var userClaims = identity.Claims;

              
                var user = new Customer
                {
                    


                    CustomerId = Convert.ToInt32(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value),
                    PassWord = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Name)?.Value,
                    FirstName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value,
                    LastName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value,
                    Phone = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.HomePhone)?.Value,
                    Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                    Roles = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value,


                };

                context.Items["User"] = user;
                await _next(context);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the middleware.");

            }
        }

    }
}











