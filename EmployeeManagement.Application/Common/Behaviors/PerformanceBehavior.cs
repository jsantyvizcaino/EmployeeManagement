using System.Diagnostics;
using Mediator;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Application.Common.Behaviors;

public sealed class PerformanceBehavior<TMessage, TResponse>(
    ILogger<PerformanceBehavior<TMessage, TResponse>> logger)
    : IPipelineBehavior<TMessage, TResponse>
    where TMessage : IMessage
{
    private const long WarningThresholdMilliseconds = 500;

    public async ValueTask<TResponse> Handle(
        TMessage message,
        MessageHandlerDelegate<TMessage, TResponse> next,
        CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        var response = await next(message, cancellationToken);
        stopwatch.Stop();

        if (stopwatch.ElapsedMilliseconds >= WarningThresholdMilliseconds)
        {
            logger.LogWarning(
                "Long-running handler: {Message} took {ElapsedMilliseconds}ms",
                typeof(TMessage).Name,
                stopwatch.ElapsedMilliseconds);
        }

        return response;
    }
}
