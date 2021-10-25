using System;
using MediatR;
using Serilog;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using SharedCore.Aplication.Interfaces;
using SharedCore.Aplication.Core.Commands;
using Device.Domain;
using SharedCore.Aplication.Payload;

namespace Device.Aplication.Shared.Behaviours {

    /// <summary>
    /// TracingBehaviour for MediatR pipeline
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class TracingBehaviour<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse> {
        private readonly ICurrentUser _currentUserService;
        private readonly ILogger _logger;

        public TracingBehaviour(
            ICurrentUser currentUserService,
             ILogger logger) {
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next ) {

            var activity = Sources.DemoSource.StartActivity(
                String.Format(
                    "TracingBehaviour: Request<{0}>",
                    typeof(TRequest).FullName),
                    ActivityKind.Server
            );

            if (typeof(TRequest).IsSubclassOf(typeof(CommandBase))) {

                ISharedCommandBase I_base_command = request as ISharedCommandBase;

                if (I_base_command.ActivityId == null
                    && Activity.Current != null
                    && Activity.Current.Id != null) {
                    I_base_command.ActivityId = Activity.Current.Id;
                }

                // This chane activity parrent / children relation..
                if (I_base_command.ActivityId != null
                    && Activity.Current != null
                    && Activity.Current.ParentId == null) {
                    Activity.Current.SetParentId(I_base_command.ActivityId);
                }
            }

            try {
                activity?.Start();
                
                return await next();

            } finally {
                activity?.Stop();
                activity?.Dispose();
            }
        }
    }
}