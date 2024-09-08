using MassTransit;

namespace MicroservicesSagas.Commons
{
    public class SubmitTransferEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
        public Guid TransactionId { get; set; }
    }

    public class ValidateTransferCommand : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }

        public Guid TransactionId { get; set; }
    }


    public class TransferValidatedEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }

        public Guid TransactionId { get; set; }

    }

    public class InvalidAmountEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }

        public Guid TransactionId { get; set; }
        public string Error { get; set; }

    }

    public class InvalidAccountEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }

        public Guid TransactionId { get; set; }
        public string Error { get; set; }

    }


    public class OtherReasonValidationFailedEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }

        public Guid TransactionId { get; set; }
        public string Error { get; set; }

    }


    public class CancelTransferCommand : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }

        public Guid TransactionId { get; set; }

    }

    public class TransferCanceledEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }

        public Guid TransactionId { get; set; }

    }
    public class TransferNotCanceledEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }

        public Guid TransactionId { get; set; }
        public string Error { get; set; }


    }

    /// <summary>
    /// ////////////////////////////////////////
    /// </summary>
    /// 


    public class TransferCommand : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }

        public Guid TransactionId { get; set; }
    }



    public class TransferSucceededEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }

        public Guid TransactionId { get; set; }
    }

    public class OtherReasonTransferFailedEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }

        public Guid TransactionId { get; set; }
        public string Error { get; set; }
    }


    /// <summary>
    /// ////////////////////////////////////////
    /// </summary>
    /// 




    public class IssueReceiptCommand : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }

        public Guid TransactionId { get; set; }
    }



    public class ReceiptIssuedEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }

        public Guid TransactionId { get; set; }
    }

    public class OtherReasonReceiptFailedEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }

        public Guid TransactionId { get; set; }
        public string Error { get; set; }
    }



}
