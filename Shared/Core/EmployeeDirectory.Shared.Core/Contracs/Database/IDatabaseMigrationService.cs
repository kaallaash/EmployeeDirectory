using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using System.Threading;
using System.Threading.Tasks;

namespace EmployeeDirectory.Shared.Core.Contracs.Database
{
    public interface IDatabaseMigrationService<in TContext>
        where TContext : DbContext
    {
        Task Migrate(
            string[] args,
            IDesignTimeDbContextFactory<TContext> factory,
            CancellationToken cancellationToken = default);
    }
}
