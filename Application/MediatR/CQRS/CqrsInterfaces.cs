using MediatR;

namespace Application.MediatR.CQRS;

public interface ICommand<TResponse> : IRequest<TResponse>
{
}

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
}

public interface IQuery<TResponse> : IRequest<TResponse>
{
}

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
}