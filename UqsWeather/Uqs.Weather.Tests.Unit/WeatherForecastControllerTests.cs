using AdamTibi.OpenWeather;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Uqs.Weather.Controllers;

namespace Uqs.Weather.Tests.Unit;

public class WeatherForecastControllerTests
{
    [Theory]
    [InlineData(-100, -148)]
    [InlineData(-10.1, 13.8)]
    [InlineData(10, 50)]
    public void ConvertCToF_ReceivesDouble_CorrectlyConverts(double c, double f)
    {

        var logger = NullLogger<WeatherForecastController>.Instance;
        var controller = new WeatherForecastController(logger, null!, null!, null!);
        double actual = controller.ConvertCToF(c);
        Assert.Equal(f, actual, 1);
    }

    [Fact]
    public async void GetReal_NotInterestedInTodayWeather_WFStartFromNextDay()
    {
        // Arrange
        const double nextDayTemp = 3.3;
        const double day5Temp = 7.7;
        var today = new DateTime(2022, 1, 1);
        var realWeatherTemps = new double[] { 2, nextDayTemp, 4, 5.5, 6, day5Temp, 8 };
        var clientStub = new ClientStub(today, realWeatherTemps);
        var clientMock = Substitute.For<IClient>();
        clientMock.OneCallAsync(Arg.Any<decimal>(), Arg.Any<decimal>(), Arg.Any<IEnumerable<Excludes>>(), Arg.Any<Units>())
                  .Returns(x =>
                  {
                      const int DAYS = 7;
                      OneCallResponse res = new()
                      {
                          Daily = new Daily[DAYS]
                      };
                      for (int i = 0; i < DAYS; i++)
                      {
                          res.Daily[i] = new Daily
                          {
                              Dt = today.AddDays(i),
                              Temp = new Temp
                              {
                                  Day = realWeatherTemps.ElementAt(i),
                              }
                          };
                      }

                      return Task.FromResult(res);
                  });

        var controller = new WeatherForecastController(null!, clientMock, null!, null!);

        // Act
        IEnumerable<WeatherForecast> wfs = await controller.GetReal();
        // Assert
        Assert.Equal(3, wfs.First().TemperatureC);
    }

    [Fact]
    public async Task GetReal_RequestsToOpenWeather_MetricUnitIsUsed()
    {
        // Arrange
        const double nextDayTemp = 3.3;
        const double day5Temp = 7.7;
        var today = new DateTime(2022, 1, 1);
        var realWeatherTemps = new double[] { 2, nextDayTemp, 4, 5.5, 6, day5Temp, 8 };
        var clientStub = new ClientStub(today, realWeatherTemps);
        var clientMock = Substitute.For<IClient>();
        clientMock.OneCallAsync(Arg.Any<decimal>(), Arg.Any<decimal>(), Arg.Any<IEnumerable<Excludes>>(), Arg.Any<Units>())
                  .Returns(x =>
                  {
                      const int DAYS = 7;
                      OneCallResponse res = new()
                      {
                          Daily = new Daily[DAYS]
                      };
                      for (int i = 0; i < DAYS; i++)
                      {
                          res.Daily[i] = new Daily
                          {
                              Dt = today.AddDays(i),
                              Temp = new Temp
                              {
                                  Day = realWeatherTemps.ElementAt(i),
                              }
                          };
                      }

                      return Task.FromResult(res);
                  });
        var controller = new WeatherForecastController(null!, clientMock, null!, null!);

        // Act
        var _ = await controller.GetReal();

        // Assert
        await clientMock.Received().OneCallAsync(
        Arg.Any<decimal>(), Arg.Any<decimal>(),
        Arg.Any<IEnumerable<Excludes>>(),
        Arg.Is<Units>(x => x == Units.Metric));
    }
}
