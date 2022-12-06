namespace Uqs.AppointmentBooking.Domain.Tests.Unit;

public class NowService : INowService
{
    public DateTime Now => DateTime.UtcNow;
}
