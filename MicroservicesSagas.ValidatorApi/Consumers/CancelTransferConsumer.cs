using MassTransit;

using MicroservicesSagas.Commons;

namespace MicroservicesSagas.ValidatorApi.Consumers
{
    public class CancelTransferConsumer : IConsumer<CancelTransferCommand>
    {
        public async Task Consume(ConsumeContext<CancelTransferCommand> context)
        {

            await context.Publish(new TransferCanceledEvent { TransactionId = context.Message.TransactionId });

        }

    }

}
