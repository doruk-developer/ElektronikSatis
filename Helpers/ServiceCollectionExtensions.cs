using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace ElektronikSatis.Helpers
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddControllersAsServices(
            this IServiceCollection services,
            Assembly assembly)
        {
            // Tüm controller'ları bul ve kaydet
            var controllerTypes = assembly.GetExportedTypes()
                .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
                .Where(t => typeof(IController).IsAssignableFrom(t)
                         || t.Name.EndsWith("Controller"));

            foreach (var type in controllerTypes)
            {
                services.AddTransient(type);
            }

            return services;
        }
    }
}