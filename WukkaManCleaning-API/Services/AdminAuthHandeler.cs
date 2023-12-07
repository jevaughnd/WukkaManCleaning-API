using Microsoft.AspNetCore.Authorization;

namespace WukkaManCleaning_API.Services
{
    public class AdminAuthHandeler: AuthorizationHandler <AdminAcess>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminAcess requirement)
        {
            if (context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement); 
            }
            return Task.CompletedTask;
        }
    }

    public class AdminAcess:IAuthorizationRequirement
    {

    }
}
