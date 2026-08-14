using System.Diagnostics;
using Mediator;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Application.Common.Behaviors;

public sealed class LoggingBehavior<TMessage, TResponse>(
    ILogger<LoggingBehavior<TMessage, TResponse>> logger)
    : IPipelineBehavior<TMessage, TResponse>
    where TMessage : IMessage
{
    public async ValueTask<TResponse> Handle(
        TMessage message,
        MessageHandlerDelegate<TMessage, TResponse> next,
        CancellationToken cancellationToken)
    {
        var messageName = typeof(TMessage).Name;
        var stopwatch = Stopwatch.StartNew();

        logger.LogInformation("Handling {Message}", messageName);
        var response = await next(message, cancellationToken);
        stopwatch.Stop();
        logger.LogInformation(
            "Handled {Message} in {ElapsedMilliseconds}ms",
            messageName,
            stopwatch.ElapsedMilliseconds);

        return response;
    }
}
