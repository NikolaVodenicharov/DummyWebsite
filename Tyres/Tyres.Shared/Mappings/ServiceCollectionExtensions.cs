using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Tyres.Shared.Mappings
{
    //TODO gather ServiceCollectionExtensions at one place ? 
    //TODO if I use this extension i have to change the documentation of the mappings
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAutoMapperProfile(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile));

            return services;
        }
    }
}
