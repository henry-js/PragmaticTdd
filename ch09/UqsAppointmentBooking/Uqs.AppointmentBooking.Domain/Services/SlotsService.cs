using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Uqs.AppointmentBooking.Domain;
using Uqs.AppointmentBooking.Domain.Database;
using Uqs.AppointmentBooking.Domain.Tests.Unit;

namespace Uqs.AppointmentBooking.Domain.Services;

public class SlotsService
{
    private readonly ApplicationContext _ctx;
    private readonly INowService _nowService;
    private readonly IOptions<ApplicationSettings> _settings;

    public SlotsService(ApplicationContext ctx, INowService nowService, IOptions<ApplicationSettings> settings)
    {
        _ctx = ctx;
        _nowService = nowService;
        _settings = settings;
    }

    public async Task<Slots> GetAvailableSlotsForEmployee(int serviceId, int employeeId)
    {
        var service = await _ctx.Services!.SingleOrDefaultAsync(x => x.Id == serviceId);

        if (service is null)
        {
            throw new ArgumentException("Record not found", nameof(serviceId));
        }

        var shifts = _ctx.Shifts!.Where(x => x.EmployeeId == employeeId);
        if (!shifts.Any()) return new Slots(Array.Empty<DaySlots>());

        return null;
    }
}
