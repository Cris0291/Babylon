using Microsoft.Extensions.Options;
using Quartz;

namespace Babylon.Modules.Channels.Infrastructure.Outbox;
internal sealed class ConfigureProcessOutboxJob(IOptions<OutboxOptions> options) : IConfigureOptions<QuartzOptions>
{
    private readonly OutboxOptions _options = options.Value;
    public void Configure(QuartzOptions options)
    {
        string jobName = typeof(ProcessOutboxJob).FullName!;

        options
            .AddJob<ProcessOutboxJob>(configure => configure.WithIdentity(jobName))
            .AddTrigger(configure =>
                 configure
                      .ForJob(jobName)
                      .WithSimpleSchedule(s => s.WithIntervalInSeconds(_options.IntervalInSeconds).RepeatForever()));
    }
}
