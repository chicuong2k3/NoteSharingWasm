namespace SharingNote.Api.Application.Messaging;

public interface IQuery<TResponse> : IRequest<Ardalis.Result.Result<TResponse>>
{
}