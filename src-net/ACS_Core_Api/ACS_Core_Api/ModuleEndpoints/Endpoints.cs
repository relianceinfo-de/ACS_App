using ACS.Business.Core.Interfaces;
using ACS.Business.Core.Services;
using ACS_Core_Api.Interface;

namespace ACS_Core_Api.ModuleEndpoints
{
    public class Endpoints : IModule
    {
        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            try
            {
                endpoints.MapPost("/login", async (IUser _user, string username, string password) =>
                {
                    var result = await _user.Login(username, password);

                    return Results.Ok(result);
                });
            }
            catch (Exception ex)
            {

            }
            return endpoints;
        }

        public IServiceCollection RegisterModule(IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddScoped<IUser, UserService>();

            return services;
        }
    }
}
