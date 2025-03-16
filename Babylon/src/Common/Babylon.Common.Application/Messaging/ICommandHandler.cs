using Babylon.Common.Domain;
using MediatR;

namespace Babylon.Common.Application.Messaging;
public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>> where TCommand : ICommand<TResponse>;
public interface ICommandHandler<in TCommnad> : IRequestHandler<TCommnad, Result> where TCommnad: ICommand;
