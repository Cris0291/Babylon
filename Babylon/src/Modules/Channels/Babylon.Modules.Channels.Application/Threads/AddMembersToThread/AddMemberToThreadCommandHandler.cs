using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Channels.CreateThread;
using Babylon.Modules.Channels.Domain.ThreadChannels;

namespace Babylon.Modules.Channels.Application.Threads.AddMembersToThread;
internal sealed class AddMemberToThreadCommandHandler(IThreadChannelMemberRepository threadChannelMemberRepository) : ICommandHandler<AddMemberToThreadCommand>
{
    public async Task<Result> Handle(AddMemberToThreadCommand request, CancellationToken cancellationToken)
    {
        List<ThreadChannelMember> threadMembers = new();

        foreach (MemberDto member in request.members)
        {
            var threadMember = ThreadChannelMember.Create(member.Id, request.ThreadId);
            threadMembers.Add(threadMember);
        }

        await threadChannelMemberRepository.Insert(threadMembers);
    }
}
