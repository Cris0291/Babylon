using FluentValidation;

namespace Babylon.Modules.Channels.Application.Channels.CreateThread;
internal sealed class CreateThreadCommandValidator : AbstractValidator<CreateThreadCommand>
{
    public CreateThreadCommandValidator()
    {
        RuleFor(t => t.ChannelName).NotNull().NotEmpty();
        RuleFor(t => t.UserName).NotNull().NotEmpty();
        RuleFor(t => t.MessageText).NotNull().NotEmpty();
        RuleFor(t => t.CreationDate).Must((obj, date) => date != default);
    }
}
