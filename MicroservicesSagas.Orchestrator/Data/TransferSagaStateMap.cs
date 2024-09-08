using MassTransit;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroservicesSagas.Orchestrator.Data
{
    public class TransferSagaStateMap :
    SagaClassMap<TransferSagaState>
    {
        protected override void Configure(EntityTypeBuilder<TransferSagaState> entity, ModelBuilder model)
        {
            entity.Property(x => x.CurrentState).HasMaxLength(64);
            entity.Property(x => x.TransactionId);

            // If using Optimistic concurrency, otherwise remove this property
            entity.Property(x => x.RowVersion).IsRowVersion();
        }
    }
}
