using Babylon.Common.Application;
using Babylon.Common.Infrastructure;
using Babylon.Common.Presentation.Endpoints;
using Babylon.Modules.Channels.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication([Babylon.Modules.Channels.Application.AssemblyReference.Assembly, Babylon.Modules.Users.Application.AssemblyReference.Assembly]);
builder.Services.AddInfrastructure(
    builder.Configuration.GetConnectionString("Database"),
    [Babylon.Modules.Channels.Infrastructure.ChannelsModule.ConfigureConsumers],
    "ImplementReddisConfig"
    );

WebApplication app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapEndpoints();

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.Run();


