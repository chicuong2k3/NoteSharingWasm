namespace SharingNote.Api.Application.Messaging;
public interface ICommandBase { }

public interface ICommand : IRequest<Result>, ICommandBase
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, ICommandBase
{
}
