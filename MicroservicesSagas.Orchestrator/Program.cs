using System.Transactions;

using MassTransit;

using MicroservicesSagas.Commons;
using MicroservicesSagas.Orchestrator;

using static MassTransit.Logging.OperationName;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMassTransit(x =>
{
    x.AddSagaStateMachine<TransferStateMachine, TransferSagaState>()
        .InMemoryRepository();


    //x.UsingRabbitMq((context, cfg) =>
    //{
    //    cfg.ConfigureEndpoints(context);
    //});

    x.UsingAzureServiceBus((context, cfg) =>
    {
        cfg.Host(builder.Configuration["AzureServiceBus:ConnectionString"]);

        cfg.ConfigureEndpoints(context);
     
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("CreateTransaction", async (IPublishEndpoint publishEndpoint) =>
{
    await publishEndpoint.Publish(new SubmitTransferEvent()
    {
        TransactionId = Guid.NewGuid(),
    });
});

app.Run();

