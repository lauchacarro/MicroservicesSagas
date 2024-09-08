using MassTransit;

using MicroservicesSagas.Commons;

namespace MicroservicesSagas.Orchestrator
{
    public class TransferStateMachine : MassTransitStateMachine<TransferSagaState>
    {
        private readonly string _serviceBusUrl;

        public State Validating { get; private set; }
        public State Transferring { get; private set; }
        public State Receiving { get; private set; }
        public State Completed { get; private set; }
        public State Failed { get; private set; }

        public Event<SubmitTransferEvent> SubmitTransfer { get; private set; }
        public Event<TransferValidatedEvent> TransferValidatedEvent { get; private set; }
        public Event<InvalidAmountEvent> InvalidAmountEvent { get; private set; }
        public Event<InvalidAccountEvent> InvalidAccountEvent { get; private set; }
        public Event<OtherReasonValidationFailedEvent> OtherReasonValidationFailedEvent { get; private set; }
        public Event<TransferCanceledEvent> TransferCanceledEvent { get; private set; }
        public Event<TransferNotCanceledEvent> TransferNotCanceledEvent { get; private set; }
        public Event<TransferSucceededEvent> TransferSucceededEvent { get; private set; }
        public Event<OtherReasonTransferFailedEvent> OtherReasonTransferFailedEvent { get; private set; }
        public Event<ReceiptIssuedEvent> ReceiptIssuedEvent { get; private set; }
        public Event<OtherReasonReceiptFailedEvent> OtherReasonReceiptFailedEvent { get; private set; }

        public TransferStateMachine(IConfiguration configuration)
        {
            _serviceBusUrl = configuration["AzureServiceBus:Url"]!;

            InstanceState(x => x.CurrentState);

            Initially(
                When(SubmitTransfer)
                    .TransitionTo(Validating)
                    .Publish(context => new ValidateTransferCommand
                    {
                        TransactionId = context.Message.TransactionId,
                        CorrelationId = context.Message.CorrelationId


                    })
            );


            During(Validating,
                When(TransferValidatedEvent)
                    .TransitionTo(Transferring)
                    .Publish(context => new TransferCommand
                    {
                        TransactionId = context.Message.TransactionId,
                        CorrelationId = context.Message.CorrelationId
                    }),
                When(InvalidAmountEvent)
                    .TransitionTo(Failed)
                    .Publish(context => new CancelTransferCommand
                    {
                        TransactionId = context.Message.TransactionId,
                        CorrelationId = context.Message.CorrelationId
                    })
            );


            During(Transferring,
                When(TransferSucceededEvent)
                    .TransitionTo(Receiving)
                    .Publish(context => new IssueReceiptCommand
                    {
                        TransactionId = context.Message.TransactionId,
                        CorrelationId = context.Message.CorrelationId
                    }),
                When(OtherReasonTransferFailedEvent)
                    .TransitionTo(Failed)
                    .Publish(context => new CancelTransferCommand
                    {
                        TransactionId = context.Message.TransactionId,
                        CorrelationId = context.Message.CorrelationId
                    })
            );

            During(Receiving,
                When(ReceiptIssuedEvent)
                    .TransitionTo(Completed),
                When(OtherReasonReceiptFailedEvent)
                    .TransitionTo(Failed)
                    .Publish(context => new CancelTransferCommand
                    {
                        TransactionId = context.Message.TransactionId,
                        CorrelationId = context.Message.CorrelationId
                    })
            );

            During(Failed,
                When(TransferCanceledEvent)
                    .TransitionTo(Completed),
                When(TransferNotCanceledEvent)
                    .TransitionTo(Failed)
            );

    
        }
    }
}
