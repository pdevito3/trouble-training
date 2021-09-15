using MediatR;
using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using APIServer.Extensions;
using APIServer.Aplication.Shared.Behaviours;
using APIServer.Aplication.Commands.WebHooks;

namespace APIServer.Configuration {
    public static partial class ServiceExtension {
        public static IServiceCollection AddMediatR(this IServiceCollection services) {

            // Command executor
            services.AddMediatR(cfg => cfg.Using<AppMediator>(), typeof(CreateWebHook).GetTypeInfo().Assembly);

            services.AddTransient<APIServer.Extensions.IPublisher, APIServer.Extensions.Publisher>();

            services.AddValidatorsFromAssembly(typeof(CreateWebHookValidator).GetTypeInfo().Assembly);

            services.MediatRScheduler();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TracingBehaviour<,>));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CommandBehaviour<,>));

            return services;
        }
    }
}