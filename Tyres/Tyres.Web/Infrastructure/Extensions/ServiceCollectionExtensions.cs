using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;
using Tyres.Service.Implementations;

namespace Tyres.Web.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// We create interface called <seealso cref="AbstractService"/> in the Service assembly. So we can find and register 
        /// the services in the <seealso cref="IServiceCollection"/>.
        /// </summary>
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            Assembly
                .GetAssembly(typeof(AbstractService))
                .GetTypes()
                .Where(
                    t => t.IsClass && 
                    t.GetInterfaces().Any(i => i.Name == $"I{t.Name}"))
                .Select(t => new
                {
                    Interface = t.GetInterface($"I{t.Name}"),
                    Implementation = t
                })
                .ToList()
                .ForEach(s => services.AddTransient(s.Interface, s.Implementation));

            return services;
        }
    }
}
