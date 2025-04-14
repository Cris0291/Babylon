using Microsoft.Extensions.Options;
using Quartz;

namespace Babylon.Modules.Users.Infrastructure.Outbox;
internal sealed class ConfigureProcessOutboxJob(IOptions<OutboxOptions> options) : IConfigureOptions<QuartOp>
{
}
