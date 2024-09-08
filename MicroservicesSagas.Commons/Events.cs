using MassTransit;

namespace MicroservicesSagas.Commons
{
    public record SubmitTransferEvent(Guid CorrelationId, Guid TransactionId) : CorrelatedBy<Guid>;

    public record ValidateTransferCommand(Guid CorrelationId, Guid TransactionId) : CorrelatedBy<Guid>;

    public record TransferValidatedEvent(Guid CorrelationId, Guid TransactionId) : CorrelatedBy<Guid>;

    public record InvalidAmountEvent(Guid CorrelationId, Guid TransactionId, string Error) : CorrelatedBy<Guid>;

    public record InvalidAccountEvent(Guid CorrelationId, Guid TransactionId, string Error) : CorrelatedBy<Guid>;

    public record OtherReasonValidationFailedEvent(Guid CorrelationId, Guid TransactionId, string Error) : CorrelatedBy<Guid>;

    public record CancelTransferCommand(Guid CorrelationId, Guid TransactionId) : CorrelatedBy<Guid>;

    public record TransferCanceledEvent(Guid CorrelationId, Guid TransactionId) : CorrelatedBy<Guid>;

    public record TransferNotCanceledEvent(Guid CorrelationId, Guid TransactionId, string Error) : CorrelatedBy<Guid>;

    public record TransferCommand(Guid CorrelationId, Guid TransactionId) : CorrelatedBy<Guid>;

    public record TransferSucceededEvent(Guid CorrelationId, Guid TransactionId) : CorrelatedBy<Guid>;

    public record OtherReasonTransferFailedEvent(Guid CorrelationId, Guid TransactionId, string Error) : CorrelatedBy<Guid>;

    public record IssueReceiptCommand(Guid CorrelationId, Guid TransactionId) : CorrelatedBy<Guid>;

    public record ReceiptIssuedEvent(Guid CorrelationId, Guid TransactionId) : CorrelatedBy<Guid>;

    public record OtherReasonReceiptFailedEvent(Guid CorrelationId, Guid TransactionId, string Error) : CorrelatedBy<Guid>;
}
