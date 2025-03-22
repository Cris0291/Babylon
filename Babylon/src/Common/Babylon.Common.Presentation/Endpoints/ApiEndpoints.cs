namespace Babylon.Common.Presentation.Endpoints;
public static class ApiEndpoints
{
    private const string Base = "api";

    public static class Channels
    {
        private const string ChannelBase = $"{Base}/channels";

        public const string CreateChannel = ChannelBase;
        public const string GetChannels = ChannelBase;
    }
}
