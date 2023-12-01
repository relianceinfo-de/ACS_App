namespace ACS_Core_Api.Interface
{
    public interface IModule
    {
        IServiceCollection RegisterModule(IServiceCollection services, ConfigurationManager configuration);
        IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints);
    }
}
