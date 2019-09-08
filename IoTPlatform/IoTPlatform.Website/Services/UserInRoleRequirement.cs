using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace IoTPlatform.Website.Services
{
    public class UserInRoleRequirement : IAuthorizationRequirement
    {
        public UserInRoleRequirement(string role)
        {
            Role = role;
        }

        public string Role { get; set; }
    }


    public class UserInRoleHandler : AuthorizationHandler<UserInRoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            UserInRoleRequirement requirement)
        {
            if (context.User.IsInRole(requirement.Role))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}