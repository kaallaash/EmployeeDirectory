using ED.Services.Employees.Context;
using EmployeeDirectory.Shared.Core.Database;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ED.Services.Employees.Migrations
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var cancellationToken = new CancellationToken();
            var service = new DatabaseMigrationService<EmployeeContext>();

            await service
                .Migrate(
                    args,
                    new DesignTimeDbContextFactory(),
                    cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
