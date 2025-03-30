using Babylon.Modules.Channels.Domain.Channels;
using FluentValidation;

namespace Babylon.Modules.Channels.Application.Channels.GetChannels;
internal sealed class GetChannelsQueryValidator : AbstractValidator<GetChannelsQuery>
{
    public GetChannelsQueryValidator()
    {
        RuleFor(query => query.Name).NotEmpty().NotNull();
        RuleFor(query => query.Type).NotEmpty().NotNull().Must(type => Enum.TryParse(typeof(ChannelType), type, out object? result))
            .WithMessage("The type of channel was not valid");
    }
}
