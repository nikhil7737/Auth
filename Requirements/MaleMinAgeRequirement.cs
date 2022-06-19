using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System;

namespace Auth.Requirements
{
    public class GenderMinAgeRequirement : IAuthorizationRequirement
    {
        public string Gender { get; }
        public int Age { get; }
        public GenderMinAgeRequirement(int age, string gender)
        {
            Gender = gender;
            Age = age;
        }
    }

    public class GenderMinAgeRequirementHandler : AuthorizationHandler<GenderMinAgeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GenderMinAgeRequirement requirement)
        {
            string userGender = context.User.FindFirst(claim => claim.Type == "gender1")?.Value;
            string userAge = context.User.FindFirst(claim => claim.Type == "age").Value;
            if (userGender == requirement.Gender && Convert.ToInt32(userAge) >= requirement.Age)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }

    public class GenderMinAgeRequirementHandler1 : AuthorizationHandler<GenderMinAgeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GenderMinAgeRequirement requirement)
        {
            string userGender = context.User.FindFirst(claim => claim.Type == "gender1")?.Value;
            string userAge = context.User.FindFirst(claim => claim.Type == "age").Value;
            if (userGender == requirement.Gender && Convert.ToInt32(userAge) == 10)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}