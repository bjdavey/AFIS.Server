using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace AFIS.Server.Helpers
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public IServiceProvider ServiceProvider { get; set; }

        public ApiKeyAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IServiceProvider serviceProvider)
            : base(options, logger, encoder, clock)
        {
            ServiceProvider = serviceProvider;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var apiKey = Request.Headers["VerificationApiKey"].FirstOrDefault();
            var VerificationApiKey = Startup.AppSettings.GetValue<string>("VerificationApiKey");
            if (apiKey == VerificationApiKey)
            {
                var identity = new ClaimsIdentity(nameof(ApiKeyAuthenticationHandler));
                var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), this.Scheme.Name);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            else
                return Task.FromResult(AuthenticateResult.Fail("Invalid VerificationApiKey"));

        }
    }


    public class ApiKeyRequirement : IAuthorizationRequirement
    {
    }

    public class ApiKeyAuthorizationHandler : AuthorizationHandler<ApiKeyRequirement>
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public ApiKeyAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext authContext, ApiKeyRequirement requirement)
        {
            var httpContext = httpContextAccessor.HttpContext;
            var apiKey = httpContext.Request.Headers["VerificationApiKey"].FirstOrDefault();
            var VerificationApiKey = Startup.AppSettings.GetValue<string>("VerificationApiKey");
            if (apiKey == VerificationApiKey)
                authContext.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
