using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Auth.Requirements
{
    public class NonAdminUserRequirement : IAuthorizationRequirement
    {

    }

    public class NonAdminUserRequirementHandler : AuthorizationHandler<NonAdminUserRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, NonAdminUserRequirement requirement)
        {
            string userRole = context.User.FindFirst(claim => claim.Type == "role1")?.Value;
            if (userRole != "admin")
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}