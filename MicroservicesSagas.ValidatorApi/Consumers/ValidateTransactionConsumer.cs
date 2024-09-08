using MassTransit;

using MicroservicesSagas.Commons;

namespace MicroservicesSagas.ValidatorApi.Consumers
{
    public class ValidateTransactionConsumer : IConsumer<ValidateTransferCommand>
    {
        public async Task Consume(ConsumeContext<ValidateTransferCommand> context)
        {

            if (await IsInvalidAmount(context.Message.TransactionId))
            {
                await context.Publish(new InvalidAmountEvent
                {
                    TransactionId = context.Message.TransactionId,
                    CorrelationId = context.Message.CorrelationId,
                    Error = "El monto de la transferencia es invalido."
                });
                return;
            }


            if (await IsInvalidAccount(context.Message.TransactionId))
            {
                await context.Publish(new InvalidAmountEvent
                {
                    TransactionId = context.Message.TransactionId,
                    CorrelationId = context.Message.CorrelationId,
                    Error = "La cuenta de la transferencia es invalida."
                });
                return;
            }


            await context.Publish(new TransferValidatedEvent { TransactionId = context.Message.TransactionId });

        }

        private Task<bool> IsInvalidAmount(Guid transactionId)
        {
            // Implement validation logic
            return Task.FromResult(false);
        }

        private Task<bool> IsInvalidAccount(Guid transactionId)
        {
            // Implement validation logic
            return Task.FromResult(false);
        }

    }

}
