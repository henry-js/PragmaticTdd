using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute.ExceptionExtensions;
using Uqs.AppointmentBooking.Domain.Services;
using Uqs.AppointmentBooking.Domain.Tests.Unit.Db;

namespace Uqs.AppointmentBooking.Domain.Tests.Unit;

public class SlotsServiceTests : IClassFixture<TestDatabaseFixture>
{
    private readonly TestDatabaseBuilder _ctxBuilder;

    private SlotsService _sut;
    private readonly INowService _nowService = Substitute.For<INowService>();
    private readonly ApplicationSettings _applicationSettings =
        new()
        {
            OpenAppointmentInDays = 7,
            RoundUpInMin = 5,
            RestInMin = 5
        };
    private readonly IOptions<ApplicationSettings> _settings = Substitute.For<IOptions<ApplicationSettings>>();

    public SlotsServiceTests()
    {
        _settings.Value.Returns(_applicationSettings);
    }
    public TestDatabaseFixture Fixture { get; }

    public SlotsServiceTests(TestDatabaseFixture fixture)
    {
        Fixture = fixture;
        _ctxBuilder = new(Fixture.CreateContext());
    }

    [Fact]
    public async Task
    GetAvailableSlotsSForEmployee_ServiceIdNoFound_ThrowsArgumentException()
    {
        // Arrange
        var ctx = _ctxBuilder.Build();
        _sut = new SlotsService(ctx, _nowService, _settings);

        // Act
        Func<Task> act = () => _sut.GetAvailableSlotsForEmployee(-1, -1);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>().WithParameterName("serviceId");
    }

    [Fact]
    public async Task GetAvailableSlotsForEmployee_NoShiftsForTomAndNoAppointmentsInSystem_ReturnsNoSlots()
    {
        // Arrange
        var appointmentFrom = new DateTime(2022, 11, 19, 7, 0, 0);
        _nowService.Now.Returns(appointmentFrom);
        var ctx = _ctxBuilder
            .WithSingleService(30)
            .WithSingleEmployeeTom()
            .Build();
        _sut = new SlotsService(ctx, _nowService, _settings);
        var tom = ctx.Employees!.Single();
        var mensCut30Min = ctx.Services!.Single();

        // Act
        var slots = await _sut.GetAvailableSlotsForEmployee(mensCut30Min.Id, tom.Id);

        // Assert
        slots.Should().NotBeNull();
        slots.DaySlots.SelectMany(x => x.Times).Should().BeEmpty();
    }

    [Theory]
    [InlineData(5, 0)]
    [InlineData(25, 0)]
    [InlineData(30, 1, "2022-10-03 09:00:00")]
    [InlineData(35, 2, "2022-10-03 09:00:00", "2022-10-03 09:05:00")]
    public async Task GetAvailableSlotsForEmployee_OneShiftAndNoExistingAppointments_VaryingSlots
    (int serviceDuration, int totalSlots, params string[] expectedTimes)
    {
        DateTime timeOfBooking = new DateTime(2022, 11, 19, 7, 0, 0);
        _nowService.Now.Returns(timeOfBooking);
        DateTime shiftFrom = new(2022, 11, 19, 9, 0, 0);
        DateTime shiftTo = shiftFrom.AddMinutes(serviceDuration);
        var ctx = _ctxBuilder
            .WithSingleService(30)
            .WithSingleEmployeeTom()
            .WithSingleShiftForTom(shiftFrom, shiftTo)
            .Build();
        _sut = new SlotsService(ctx, _nowService, _settings);
        var tom = ctx.Employees!.Single();
        var mensCut30Min = ctx.Services!.Single();

    }

}
