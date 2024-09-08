using MassTransit;

namespace MicroservicesSagas.Orchestrator
{
    public class TransferSagaState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public Guid TransactionId { get; set; }
    }
}
