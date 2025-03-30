using FluentValidation;

namespace Babylon.Modules.Channels.Application.Channels.AddMembersToChannel;
internal sealed class AddMembersToChannelCommandValidator : AbstractValidator<AddMembersToChannelCommand>
{
    public AddMembersToChannelCommandValidator()
    {
        RuleFor(command => command.MembersIds).Must((_, members) =>
        {
            var hashMap = new Dictionary<Guid, int> { };
            bool ans = true;

            foreach (Guid member in members)
            {
                hashMap[member]++;
            }

            foreach (KeyValuePair<Guid, int> item in hashMap)
            {
                if (item.Value > 1)
                {
                    ans = false;
                    break;
                }
            }

            return ans;
        }).WithMessage("Cannot add the same user twice. Please add each member of the channel once");
    }
}
