using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UserManagementAPI.Attributes;

namespace UserManagementAPI.Middlewares
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private const string SecretKey = "ThisIsASecretKeyForJwtToken12345"; // Same key as used in token generation

        public TokenValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {

             // Check if the endpoint has the SkipAuth attribute
            var endpoint = context.GetEndpoint();
            if (endpoint != null && endpoint.Metadata.GetMetadata<SkipAuthAttribute>() != null)
            {
                // Skip token validation for endpoints with the SkipAuth attribute
                await _next(context);
                return;
            }

            // Apply token validation only for POST, PUT, DELETE requests
            if (context.Request.Method == HttpMethods.Post ||
                context.Request.Method == HttpMethods.Put ||
                context.Request.Method == HttpMethods.Delete)
            {
                if (!context.Request.Headers.ContainsKey("Authorization"))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Authorization header is missing.");
                    return;
                }

                var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token) || !ValidateToken(token))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Invalid or expired token.");
                    return;
                }
            }

            // Proceed to the next middleware for other requests
            await _next(context);
        }

        private bool ValidateToken(string token)
        {
            try
            {
                var key = Encoding.ASCII.GetBytes(SecretKey);
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };

                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
