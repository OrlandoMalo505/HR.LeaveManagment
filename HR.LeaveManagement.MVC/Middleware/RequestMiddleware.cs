using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Services.Base;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Build.Framework;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace HR.LeaveManagement.MVC.Middleware
{
    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILocalStorageService _storageService;

        public RequestMiddleware(RequestDelegate next, ILocalStorageService storageService)
        {
            _next = next;
            _storageService = storageService;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                var ep = httpContext.Features.Get<IEndpointFeature>()?.Endpoint;
                var authAttr = ep?.Metadata?.GetMetadata<AuthorizeAttribute>();

                if (authAttr is not null)
                {
                    var tokenExists = _storageService.Exists("token");
                    var tokenIsValid = true;

                    if (tokenExists)
                    {
                        var token = _storageService.GetStorageValue<string>("token");
                        JwtSecurityTokenHandler tokenHandler = new();
                        var tokenContent = tokenHandler.ReadJwtToken(token);
                        var expiration = tokenContent.ValidTo;
                        if (expiration < DateTime.Now)
                        {
                            tokenIsValid = false;
                        }
                    }

                    if (tokenExists == false || tokenIsValid == false)
                    {
                        await SignOutAndRedirect(httpContext);
                        return;
                    }

                    if(authAttr.Roles != null)
                    {
                        var userRole = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
                        if(authAttr.Roles.Contains(userRole) == false)
                        {
                            var path = $"/home/notauthorized";
                            httpContext.Response.Redirect(path);
                            return;
                        }                                        
                    }
                }

                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            switch (ex)
            {
                case ApiException apiException:
                    await SignOutAndRedirect(httpContext);
                    break;
                default:
                    var path = $"/Home/Error";
                    httpContext.Response.Redirect(path);
                    break;
            }
        }
        private static async Task SignOutAndRedirect(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var path = $"/users/login";
            httpContext.Response.Redirect(path);
        }
    }
}
