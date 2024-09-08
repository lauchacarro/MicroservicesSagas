using MassTransit;

using MicroservicesSagas.Commons;

namespace MicroservicesSagas.TransferApi.Consumers
{
    public class TransferConsumer : IConsumer<TransferCommand>
    {
        public async Task Consume(ConsumeContext<TransferCommand> context)
        {
            var transferResult = await TransferAsync(context.Message.TransactionId);

            if (transferResult.Success)
            {
                await context.Publish(new TransferSucceededEvent(context.Message.CorrelationId, context.Message.TransactionId));
            }
            else
            {
                await context.Publish(new OtherReasonTransferFailedEvent(context.Message.CorrelationId, context.Message.TransactionId, "Transfer failed"));
            }
        }

        private Task<TransferResult> TransferAsync(Guid transactionId)
        {
            // Implement transfer logic


            return Task.FromResult(new TransferResult(true));
        }


        record TransferResult(bool Success);
    }

}
