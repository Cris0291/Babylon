﻿namespace Babylon.Modules.Channels.Domain.Channels;
public interface IChannelRepository
{
    Task Insert(Channel channel);
}
