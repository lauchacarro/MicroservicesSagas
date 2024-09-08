using System.Reflection;

using MassTransit;

using MicroservicesSagas.Commons;
using MicroservicesSagas.Orchestrator;
using MicroservicesSagas.Orchestrator.Data;

using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMassTransit(x =>
{
    x.AddSagaStateMachine<TransferStateMachine, TransferSagaState>()
     .EntityFrameworkRepository(r =>
     {
         r.ConcurrencyMode = ConcurrencyMode.Optimistic;

         r.AddDbContext<DbContext, TransferSagaDbContext>((provider, builderEf) =>
         {
             builderEf.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"), m =>
             {
                 m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                 m.MigrationsHistoryTable($"__{nameof(TransferSagaDbContext)}");
             });
         });
     });


    x.UsingAzureServiceBus((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("AzureServiceBus"));

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
    await publishEndpoint.Publish(new SubmitTransferEvent(Guid.NewGuid(), Guid.NewGuid()));
});

app.Run();

