namespace SharingNote.Api.Application.Messaging;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{
}

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, Ardalis.Result.Result<TResponse>>
    where TCommand : ICommand<TResponse>
{
}
