using MassTransit;

using MicroservicesSagas.Commons;

namespace MicroservicesSagas.TransferApi.Consumers
{
    public class CancelTransferConsumer : IConsumer<CancelTransferCommand>
    {
        public async Task Consume(ConsumeContext<CancelTransferCommand> context)
        {

            await context.Publish(new TransferCanceledEvent(context.Message.CorrelationId, context.Message.TransactionId));

        }

    }

}
