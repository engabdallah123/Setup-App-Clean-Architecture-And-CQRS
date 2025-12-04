using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class AppDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services) 
        {
            // Add application services here
            services.AddMediatR((typeof(AppDependencyInjection).Assembly));
            return services;
        }
    }
}
