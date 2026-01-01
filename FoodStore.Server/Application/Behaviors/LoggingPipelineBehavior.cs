using Azure.Core;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodStore.Server.Application.Behaviors;

public class LoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger;
    public LoggingPipelineBehavior(ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Pre-Processing
        _logger.LogInformation("Handling {@RequestName} at {@DateTimeUtcNow}", typeof(TRequest).Name, DateTime.UtcNow);

        var response = await next(); // Excuting Handler

        // Post-Processing
        if (response.IsError)
        {
            _logger.LogError(
                "Request {RequestName} returned errors: {@Errors}",
                typeof(TRequest).Name,
                response.Errors);

        }
        else
        {
            _logger.LogInformation(
                "Request {@RequestName} completed successfully at: {@DateTimeUtcNow}",
                typeof(TRequest).Name,
                DateTime.UtcNow);
        }

        return response;
    }
}
