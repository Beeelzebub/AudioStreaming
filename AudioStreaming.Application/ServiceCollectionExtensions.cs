﻿using Microsoft.Extensions.DependencyInjection;
using MediatR;
using FluentValidation;
using AudioStreaming.Application.Mediator.Behaviors;

namespace AudioStreaming.Application
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(DummyClassForGettingApplicationAssembly).Assembly);
            services.AddMediatR(typeof(DummyClassForGettingApplicationAssembly).Assembly);
            services.AddValidatorsFromAssembly(typeof(DummyClassForGettingApplicationAssembly).Assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
        }
    }
}
