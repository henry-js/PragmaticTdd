using Microsoft.EntityFrameworkCore;

namespace Uqs.AppointmentBooking.Domain.Tests.Unit.Db;

public class TestDatabaseFixture
{
    private const string ConnectionString = @$"Server=(localdb)\mssqllocaldb;Database=AppointmentBookingTestDb;Trusted_Connection=True";

    private static readonly object _lock = new();
    private static bool _databaseInitialized;

    public TestDatabaseFixture()
    {
        lock (_lock)
        {
            if (!_databaseInitialized)
            {
                using (var context = CreateContext())
                {
                    // context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();
                }

                _databaseInitialized = true;
            }
        }
    }

    public ApplicationContext CreateContext()
        => new ApplicationContext(
            new DbContextOptionsBuilder<ApplicationContext>()
                .UseSqlServer(ConnectionString)
                .Options);
}

// public class TestApplicationContext : ApplicationContext
// {
//     public TestApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
//     {
//         options = new DbContextOptionsBuilder<ApplicationContext>().UseSqlServer()
//     }
// }
