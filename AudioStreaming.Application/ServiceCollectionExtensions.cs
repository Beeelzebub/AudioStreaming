using Microsoft.Extensions.DependencyInjection;
using MediatR;
using FluentValidation;
using AudioStreaming.Application.Mediator.Behaviors;
using AutoMapper;

namespace AudioStreaming.Application
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(DummyClassForGettingApplicationAssembly).Assembly);
            services.AddMediatR(typeof(DummyClassForGettingApplicationAssembly).Assembly);
            services.AddValidatorsFromAssembly(typeof(DummyClassForGettingApplicationAssembly).Assembly);
            
            services.AddScoped<IMapper>(provider => provider.GetService<IMapper>());

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
        }
    }
}
