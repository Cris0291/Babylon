using Babylon.Common.Domain;
using MediatR;

namespace Babylon.Common.Application.Messaging;
public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
