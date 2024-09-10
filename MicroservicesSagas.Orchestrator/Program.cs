using System.Reflection;

using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;

using MicroservicesSagas.Commons;
using MicroservicesSagas.Orchestrator;
using MicroservicesSagas.Orchestrator.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddDbContext<TransferSagaDbContext>((provider, options) =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"), m =>
    {
        m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
        m.MigrationsHistoryTable($"__{nameof(TransferSagaDbContext)}");
    });
});

builder.Services.AddMassTransit(x =>
{
    x.AddSagaStateMachine<TransferStateMachine, TransferSagaState>()
     .EntityFrameworkRepository(r =>
     {
         r.ConcurrencyMode = ConcurrencyMode.Optimistic;
         r.ExistingDbContext<TransferSagaDbContext>();
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
    var payload = new SubmitTransferEvent(Guid.NewGuid(), Guid.NewGuid());
    await publishEndpoint.Publish(payload);

    return Results.Ok(payload);
});

app.MapGet("Transactions/{id}", async (Guid id, [FromServices]TransferSagaDbContext context   ) =>
{
    var transfer = await context.Transfers.FirstOrDefaultAsync(x => x.TransactionId == id);

    if (transfer is null)
        return Results.NotFound();

    return Results.Ok(transfer);

});

app.Run();

