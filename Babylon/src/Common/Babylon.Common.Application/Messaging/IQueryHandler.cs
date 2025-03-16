using Babylon.Common.Domain;
using MediatR;

namespace Babylon.Common.Application.Messaging;
public interface IQueryHandler<in TQuery,TResponse> : IRequestHandler<TQuery, Result<TResponse>> where TQuery : IQuery<TResponse>;

