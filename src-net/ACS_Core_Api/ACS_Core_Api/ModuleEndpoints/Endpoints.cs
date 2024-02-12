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

                endpoints.MapPost("/startchat", async (HttpContext _context,IChatSender _chat) =>
                {
                    var loggedInUser = _context.User.Identity.Name;
                    var name = _context.User.Claims.FirstOrDefault(c => c.Type == "name");
                    var result = await _chat.AppChatStart(name.Value);

                    return Results.Ok(result);
                }).RequireAuthorization();

                endpoints.MapPost("/sendMessage", async (HttpContext _context, IChatSender _chat)
                    =>
                {
                    var loggedInUser = _context.User.Identity.Name;
                    var name = _context.User.Claims.FirstOrDefault(c => c.Type == "name");
                    var result = await _chat.SendChat(string content);

                }).RequireAuthorization();
            }
            catch (Exception ex)
            {

            }
            return endpoints;
        }

        public IServiceCollection RegisterModule(IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddScoped<IUser, UserService>();
            services.AddScoped<IChatSender, ChatService>();

            return services;
        }
    }
}
