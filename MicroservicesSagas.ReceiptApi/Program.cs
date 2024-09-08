using MassTransit;

using MicroservicesSagas.Commons;
using MicroservicesSagas.ReceiptApi.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddMassTransit(x =>
{

    x.AddConsumer<ReceiptConsumer>();
    x.AddConsumer<CancelTransferConsumer>();


    x.UsingAzureServiceBus((context, cfg) =>
    {


        cfg.Host(builder.Configuration["AzureServiceBus:ConnectionString"]);




        cfg.SubscriptionEndpoint<IssueReceiptCommand>("receipt-api", e =>
        {
            e.ConfigureConsumer<ReceiptConsumer>(context);
        });

        cfg.SubscriptionEndpoint<CancelTransferCommand>("receipt-api", e =>
        {
            e.ConfigureConsumer<CancelTransferConsumer>(context);
        });


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



app.Run();
