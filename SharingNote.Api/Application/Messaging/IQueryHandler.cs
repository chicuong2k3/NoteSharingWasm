namespace SharingNote.Api.Application.Messaging;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Ardalis.Result.Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
