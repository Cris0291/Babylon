using Babylon.Common.Domain;
using MediatR;

namespace Babylon.Common.Application.Messaging;
public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand;
public interface ICommand : IRequest<Result>, IBaseCommand;
public interface IBaseCommand;

