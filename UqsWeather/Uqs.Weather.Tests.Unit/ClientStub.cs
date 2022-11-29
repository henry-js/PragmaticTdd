using System.Collections;
using AdamTibi.OpenWeather;

namespace Uqs.Weather;

public class ClientStub : IClient
{
    private readonly DateTime _now;
    private readonly IEnumerable<double> _sevenDaysTemps;
    public Units? LastUnitSpy { get; set; }
    public ClientStub(DateTime now, IEnumerable<double> sevenDaysTemps)
    {
        _now = now;
        _sevenDaysTemps = sevenDaysTemps;
    }


    public Task<OneCallResponse> OneCallAsync(decimal latitude, decimal longitude, IEnumerable<Excludes> excludes, Units unit)
    {
        LastUnitSpy = unit;
        const int DAYS = 7;
        OneCallResponse res = new OneCallResponse
        {
            Daily = new Daily[DAYS]
        };
        for (int i = 0; i < DAYS; i++)
        {
            res.Daily[i] = new Daily
            {
                Dt = _now.AddDays(i),
                Temp = new Temp
                {
                    Day = _sevenDaysTemps.ElementAt(i),
                }
            };
        }
        return Task.FromResult(res);
    }
}
