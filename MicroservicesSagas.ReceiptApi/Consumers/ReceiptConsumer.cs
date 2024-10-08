﻿using MassTransit;

using MicroservicesSagas.Commons;

namespace MicroservicesSagas.ReceiptApi.Consumers
{
    public class ReceiptConsumer : IConsumer<IssueReceiptCommand>
    {
        public async Task Consume(ConsumeContext<IssueReceiptCommand> context)
        {
            var receiptResult = await RecordReceiptAsync(context.Message.TransactionId);

            if (receiptResult.Success)
            {
                await context.Publish(new ReceiptIssuedEvent(context.Message.CorrelationId, context.Message.TransactionId));
            }
            else
            {
                await context.Publish(new OtherReasonReceiptFailedEvent(context.Message.CorrelationId, context.Message.TransactionId, "Receipt recording failed"));
            }
        }

        private Task<ReceiptResult> RecordReceiptAsync(Guid transactionId)
        {
            // Implement receipt recording logic
            return Task.FromResult(new ReceiptResult(true));

        }

        record ReceiptResult(bool Success);
    }

}
