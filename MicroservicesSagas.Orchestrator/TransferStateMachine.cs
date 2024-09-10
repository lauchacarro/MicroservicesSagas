using MassTransit;

using MicroservicesSagas.Commons;

namespace MicroservicesSagas.Orchestrator
{
    public class TransferStateMachine : MassTransitStateMachine<TransferSagaState>
    {
        public State Validating { get; private set; }
        public State Transferring { get; private set; }
        public State Receiving { get; private set; }
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

        public TransferStateMachine()
        {

            InstanceState(x => x.CurrentState);

            Initially(
                When(SubmitTransfer)
                    .Then(x => x.Saga.CreatedAt = DateTime.Now)
                    .Then(x => x.Saga.TransactionId = x.Message.TransactionId)
                    .TransitionTo(Validating)
                    .Publish(context => new ValidateTransferCommand(context.Message.CorrelationId, context.Message.TransactionId))
            );


            During(Validating,
                When(TransferValidatedEvent)
                    .TransitionTo(Transferring)
                    .Publish(context => new TransferCommand(context.Message.CorrelationId, context.Message.TransactionId)),
                When(InvalidAmountEvent)
                    .TransitionTo(Failed)
                    .Publish(context => new CancelTransferCommand(context.Message.CorrelationId, context.Message.TransactionId)),
                When(InvalidAccountEvent)
                    .TransitionTo(Failed)
                    .Publish(context => new CancelTransferCommand(context.Message.CorrelationId, context.Message.TransactionId))
            );


            During(Transferring,
                When(TransferSucceededEvent)
                    .TransitionTo(Receiving)
                    .Publish(context => new IssueReceiptCommand(context.Message.CorrelationId, context.Message.TransactionId)),
                When(OtherReasonTransferFailedEvent)
                    .TransitionTo(Failed)
                    .Publish(context => new CancelTransferCommand(context.Message.CorrelationId, context.Message.TransactionId))
            );

            During(Receiving,
                When(ReceiptIssuedEvent)
                    .Finalize(),
                When(OtherReasonReceiptFailedEvent)
                    .TransitionTo(Failed)
                    .Publish(context => new CancelTransferCommand(context.Message.CorrelationId, context.Message.TransactionId))
            );

            During(Failed,
                When(TransferCanceledEvent)
                    .Finalize(),
                When(TransferNotCanceledEvent)
                    .TransitionTo(Failed)
            );


        }
    }
}
