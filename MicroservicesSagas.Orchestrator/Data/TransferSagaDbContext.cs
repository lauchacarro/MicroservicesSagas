using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace MicroservicesSagas.Orchestrator.Data
{
    public class TransferSagaDbContext :
    SagaDbContext
    {
        public TransferSagaDbContext(DbContextOptions<TransferSagaDbContext> options)
            : base(options)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("saga");
        }

        protected override IEnumerable<ISagaClassMap> Configurations
        {
            get { yield return new TransferSagaStateMap(); }
        }
    }
}
