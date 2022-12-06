using Microsoft.EntityFrameworkCore;
using Uqs.AppointmentBooking.Domain;
using Uqs.AppointmentBooking.Domain.Database;
using Uqs.AppointmentBooking.Domain.Tests.Unit;

namespace Uqs.AppointmentBooking.Domain.Services;

public class SlotsService
{
    private readonly ApplicationContext _ctx;
    private readonly INowService _nowService;
    private readonly ApplicationSettings _settings;

    public SlotsService(ApplicationContext ctx, INowService nowService, ApplicationSettings settings)
    {
        _ctx = ctx;
        _nowService = nowService;
        _settings = settings;
    }

    public async Task<Slots> GetAvailableSlotsForEmployee(int serviceId)
    {
        var service = await _ctx.Services!.SingleOrDefaultAsync(x => x.Id == serviceId);

        if (service is null)
        {
            throw new ArgumentException("Record not found", nameof(serviceId));
        }
        return null;
    }
}
