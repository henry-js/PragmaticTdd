using Uqs.AppointmentBooking.Domain.Services;
using Uqs.AppointmentBooking.Domain.Tests.Unit.Db;

namespace Uqs.AppointmentBooking.Domain.Tests.Unit;


public class ServicesServiceTests : IClassFixture<TestDatabaseFixture>
{
    private readonly TestDatabaseBuilder _ctxBuilder;
    private ServicesService? _sut;
    public TestDatabaseFixture Fixture { get; }

    public ServicesServiceTests(TestDatabaseFixture fixture)
    {
        Fixture = fixture;
        _ctxBuilder = new(Fixture.CreateContext());
    }

    [Fact]
    public async Task
    GetActiveServices_NoServiceInTheSystem_NoServices()
    {
        // Arrage
        var context = _ctxBuilder.Build();
        _sut = new ServicesService(context);

        // Act
        var actual = await _sut.GetActiveServices();

        // Assert
        Assert.True(!actual.Any());
    }

    [Fact]
    public async Task
    GetActiveServices_TwoActiveOneInactiveService_TwoServices()
    {
        // Arrange
        var context = _ctxBuilder
            .WithSingleService(true)
            .WithSingleService(true)
            .WithSingleService(false)
            .Build();
        _sut = new ServicesService(context);
        var expected = 2;

        // Act
        var actual = await _sut.GetActiveServices();

        // Assert
        Assert.Equal(expected, actual.Count());
    }
}
