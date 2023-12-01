using ACS_Core_Api.Interface;

namespace ACS_Core_Api.Module.Extension
{
    public static class ModuleExtension
    {
        static readonly List<IModule> registeredModules = new List<IModule>();

        public static IServiceCollection RegisterModules(this IServiceCollection services, ConfigurationManager configuration)
        {
            var modules = DiscoverModules();

            foreach (var module in modules)
            {
                module.RegisterModule(services, configuration);

                registeredModules.Add(module);
            }

            return services;
        }

        public static IEnumerable<IModule> DiscoverModules()
        {
            return typeof(IModule).Assembly
                .GetTypes()
                .Where(p => p.IsClass && p.IsAssignableTo(typeof(IModule)))
                .Select(Activator.CreateInstance)
                .Cast<IModule>();
        }

        public static WebApplication MapEndpoints (this WebApplication app)
        {
            foreach (var module in registeredModules)
            {
                module.MapEndpoints(app);
            }

            return app;
        }
    }
}
