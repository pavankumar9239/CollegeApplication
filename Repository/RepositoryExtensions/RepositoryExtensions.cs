using Microsoft.Extensions.DependencyInjection;
using Repository.Contracts;
using Repository.Implementations;

namespace Repository.RepositoryExtensions
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddRepositoriesExtensions(this IServiceCollection services)
        {
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped(typeof(ICollegeRepository<>), typeof(CollegeRepository<>));

            return services;
        }
    }
}
