﻿using Babylon.Common.Application.Messaging;
using Babylon.Modules.Channels.Domain.Channels;

namespace Babylon.Modules.Channels.Application.Channels.GetChannels;
public sealed record GetChannelsQuery(string Name, string Type) : IQuery<IEnumerable<ChannelsResponse>>;

