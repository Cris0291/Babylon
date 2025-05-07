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
        public const string DeleteMemberFromChannel = $"{ChannelBase}/{{channelId:guid}}/members/{{memberId:guid}}";
        public const string ChangeChannelType = $"{ChannelBase}/{{id:guid}}/type";
        public const string CreateThread = $"{ChannelBase}/{{id:guid}}/threads";
    }
    public static class Users
    {
        private const string UserBase = $"{Base}/users";

        public const string RegisterUser = $"{UserBase}/register";
    }
}
