using FluentAssertions;
using NSubstitute.ExceptionExtensions;
using Uqs.AppointmentBooking.Domain.Services;
using Uqs.AppointmentBooking.Domain.Tests.Unit.Db;

namespace Uqs.AppointmentBooking.Domain.Tests.Unit;

public class SlotsServiceTests : IClassFixture<TestDatabaseFixture>
{
    private readonly TestDatabaseBuilder _ctxBuilder;

    private SlotsService _sut;
    private readonly INowService _nowService = Substitute.For<INowService>();
    private readonly ApplicationSettings _settings =
        new()
        {
            OpenAppointmentInDays = 7,
            RoundUpInMin = 5,
            RestInMin = 5
        };

    public TestDatabaseFixture Fixture { get; }

    public SlotsServiceTests(TestDatabaseFixture fixture)
    {
        Fixture = fixture;
        _ctxBuilder = new(Fixture.CreateContext());
    }

    [Fact]
    public async Task
    GetAvailableSlotsSForEmployee_ServiceIdNoFound_ArgumentException()
    {
        // Arrange
        var ctx = _ctxBuilder.Build();
        _sut = new SlotsService(ctx, _nowService, _settings);

        // Act
        Func<Task> act = () => _sut.GetAvailableSlotsForEmployee(-1);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>().WithParameterName("serviceId");
    }
}
