namespace Babylon.Common.Presentation.Endpoints;
public static class ApiEndpoints
{
    private const string Base = "api";

    public static class Channels
    {
        private const string ChannelBase = $"{Base}/channels";

        public const string CreateChannel = ChannelBase;
        public const string GetChannels = ChannelBase;
        public const string AddMembersToChannel = $"{ChannelBase}/{{id:guid}}/members";
        public const string DeleteMemberFromChannel = $"{ChannelBase}/{{channelId:guid}}/members/{{id:guid}}";
        public const string ChangeChannelType = $"{ChannelBase}/{{id:guid}}/type";
        public const string CreateThread = $"{ChannelBase}/{{id:guid}}/threads";
        public const string ArchiveChannel = $"{ChannelBase}/{{id:guid}}/type-archive";
        public const string RenameChannel = $"{ChannelBase}/{{id:guid}}/name";
        public const string DeleteChannel = $"{ChannelBase}/{{id:guid}}";
        public const string MuteMember = $"{ChannelBase}/{{channelId:guid}}/members/{{id:guid}}";
        public const string ListBlockedMembers = $"{ChannelBase}/{{channelId:guid}}/members/{{id:guid}}";
        public const string SearchChannelMessages = $"{ChannelBase}/{{channelId:guid}}/messages";
        public const string PinChannelMessage = $"{ChannelBase}/{{channelId:guid}}/messages/{{messageChannelId:guid}}";
        public const string PinThreadChannelMessage = $"{ChannelBase}/{{channelId:guid}}/threads/{{threadChannelId:guid}}/messages/{{messageThreadChannelId:guid}}";
    }
    public static class Users
    {
        private const string UserBase = $"{Base}/users";

        public const string RegisterUser = $"{UserBase}/register";
    }
    public static class DirectChannels
    {
        private const string DirectChannelBase = $"{Base}/direct-channels";

        public const string CreateDirectChannel = DirectChannelBase;
    }
}
