namespace Uqs.Weather.Wrappers;

[ExcludeFromCodeCoverage]
public class RandomWrapper : IRandomWrapper
{
    private readonly Random _random = Random.Shared;

    public int Next(int minValue, int maxValue)
    {
        return _random.Next(minValue, maxValue);
    }
}
