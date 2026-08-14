using EmployeeManagement.Domain.Dtos;
using FluentValidation;
using Mediator;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Application.Common.Behaviors;

public sealed class ValidationBehavior<TMessage, TResponse>(
    ILogger<ValidationBehavior<TMessage, TResponse>> logger,
    IEnumerable<IValidator<TMessage>> validators)
    : IPipelineBehavior<TMessage, TResponse>
    where TMessage : IMessage
{
    public async ValueTask<TResponse> Handle(
        TMessage message,
        MessageHandlerDelegate<TMessage, TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
            return await next(message, cancellationToken);

        var context = new ValidationContext<TMessage>(message);
        var validationResults = await Task.WhenAll(
            validators.Select(validator =>
                validator.ValidateAsync(context, cancellationToken)));
        var failures = validationResults
            .SelectMany(result => result.Errors)
            .Where(failure => failure is not null)
            .ToList();

        if (failures.Count == 0)
            return await next(message, cancellationToken);

        var details = string.Join(
            Environment.NewLine,
            failures.Select(failure => failure.ErrorMessage).Distinct());

        logger.LogWarning(
            "Validation failed for {Message}: {ValidationErrors}",
            typeof(TMessage).Name,
            details);

        var invalidResult = EmptyResult.InvalidRequest(details);
        if (Activator.CreateInstance(typeof(TResponse)) is EmptyResultDto response)
        {
            response.Succeed = invalidResult.Succeed;
            response.Message = invalidResult.Message;
            response.MessageId = invalidResult.MessageId;
            response.MessageType = invalidResult.MessageType;
            return (TResponse)(object)response;
        }

        throw new ValidationException(failures);
    }
}
