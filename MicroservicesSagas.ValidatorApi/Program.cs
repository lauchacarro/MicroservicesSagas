using MassTransit;

using MicroservicesSagas.Commons;
using MicroservicesSagas.ValidatorApi.Consumers;

using static MassTransit.Logging.OperationName;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMassTransit(x =>
{

    x.AddConsumer<ValidateTransactionConsumer>();


    x.UsingAzureServiceBus((context, cfg) =>
    {


        cfg.Host(builder.Configuration["AzureServiceBus:ConnectionString"]);




        cfg.SubscriptionEndpoint<ValidateTransferCommand>("validator-api", e =>
        {
            e.ConfigureConsumer<ValidateTransactionConsumer>(context);
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

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
