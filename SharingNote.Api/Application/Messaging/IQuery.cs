namespace SharingNote.Api.Application.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}