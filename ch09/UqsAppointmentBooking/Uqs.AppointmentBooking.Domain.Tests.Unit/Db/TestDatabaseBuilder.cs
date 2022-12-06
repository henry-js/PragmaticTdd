using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Uqs.AppointmentBooking.Domain.DomainObjects;

namespace Uqs.AppointmentBooking.Domain.Tests.Unit.Db;

public class TestDatabaseBuilder : IDisposable
{
    public TestDatabaseBuilder(ApplicationContext context)
    {
        _context = context;
    }
    private readonly ApplicationContext _context;

    private EntityEntry<Employee> _tomEmp;
    private EntityEntry<Employee> _janeEmp;
    private EntityEntry<Employee> _willEmp;
    private EntityEntry<Employee> _jessEmp;
    private EntityEntry<Employee> _edEmp;
    private EntityEntry<Employee> _oliEmp;

    private EntityEntry<Customer> _paulCust;
    private EntityEntry<Service> _mensCut;
    private EntityEntry<Appointment> _appointmentForPaulWithTom;
    private bool TransactionStarted = false;

    public TestDatabaseBuilder WithSingleEmployeeTom()
    {
        if (!TransactionStarted)
        {
            _context.Database.BeginTransaction();
            TransactionStarted = true;
        }
        _tomEmp = _context.Add(new Employee { Name = "Thomas Fringe" });
        return this;
    }

    public TestDatabaseBuilder WithEmployees()
    {
        if (!TransactionStarted)
        {
            _context.Database.BeginTransaction();
            TransactionStarted = true;
        }
        _tomEmp = _context.Add(new Employee { Name = "Thomas Fringe" });
        _janeEmp = _context.Add(new Employee { Name = "Jane Haircomb" });
        _willEmp = _context.Add(new Employee { Name = "William Scissors" });
        _jessEmp = _context.Add(new Employee { Name = "Jessica Clipper" });
        _edEmp = _context.Add(new Employee { Name = "Edward Sideburn" });
        _oliEmp = _context.Add(new Employee { Name = "Oliver Bold" });
        return this;
    }

    public TestDatabaseBuilder WithSingleShiftForTom(DateTime from, DateTime to)
    {
        if (!TransactionStarted)
        {
            _context.Database.BeginTransaction();
            TransactionStarted = true;
        }
        _context.Add(new Shift { Employee = _tomEmp.Entity, Starting = from, Ending = to });
        return this;
    }

    public TestDatabaseBuilder WithSingleService(short min)
    {
        if (!TransactionStarted)
        {
            _context.Database.BeginTransaction();
            TransactionStarted = true;
        }
        _mensCut = _context.Add(new Service
        { Name = "Men's Cut", AppointmentTimeSpanInMin = min, Price = 23, IsActive = true });
        return this;
    }

    public TestDatabaseBuilder WithSingleService(bool isActive)
    {
        if (!TransactionStarted)
        {
            _context.Database.BeginTransaction();
            TransactionStarted = true;
        }
        _context.Add(new Service { IsActive = isActive });
        return this;
    }

    public TestDatabaseBuilder WithSingleCustomerPaul()
    {
        if (!TransactionStarted)
        {
            _context.Database.BeginTransaction();
            TransactionStarted = true;
        }
        _paulCust = _context.Add(new Customer { FirstName = "Paul", LastName = "Longhair" });
        return this;
    }

    public TestDatabaseBuilder WithSingleAppointmentForTom(DateTime from, DateTime to)
    {
        if (!TransactionStarted)
        {
            _context.Database.BeginTransaction();
            TransactionStarted = true;
        }
        _appointmentForPaulWithTom = _context.Add(new Appointment
        {
            Service = _mensCut.Entity
            ,
            Customer = _paulCust.Entity,
            Employee = _tomEmp.Entity,
            Starting = from,
            Ending = to
        });
        return this;
    }

    public TestDatabaseBuilder WithServices()
    {
        if (!TransactionStarted)
        {
            _context.Database.BeginTransaction();
            TransactionStarted = true;
        }
        var mensCut = _context.Add(new Service
        { Name = "Men's Cut", AppointmentTimeSpanInMin = 30, Price = 23, IsActive = true });
        var mensClipperScissor = _context.Add(new Service
        { Name = "Men - Clipper & Scissor Cut", AppointmentTimeSpanInMin = 30, Price = 23, IsActive = true });
        var mensBeardTrim = _context.Add(new Service
        { Name = "Men - Beard Trim", AppointmentTimeSpanInMin = 10, Price = 10, IsActive = true });
        var mensColoring = _context.Add(new Service
        { Name = "Men - Full Head Coloring", AppointmentTimeSpanInMin = 70, Price = 60, IsActive = true });
        var mensPerm = _context.Add(new Service
        { Name = "Men - Perm", AppointmentTimeSpanInMin = 100, Price = 90, IsActive = true });
        var mensKeratin = _context.Add(new Service
        { Name = "Men - Keratin Treatment", AppointmentTimeSpanInMin = 120, Price = 100, IsActive = false });
        var boysCut = _context.Add(new Service
        { Name = "Boys - Cut", AppointmentTimeSpanInMin = 30, Price = 15, IsActive = true });
        var girlsCut = _context.Add(new Service
        { Name = "Girls - Cut", AppointmentTimeSpanInMin = 30, Price = 17, IsActive = true });

        return this;
    }

    public ApplicationContext Build()
    {
        _context.SaveChanges();
        _context.ChangeTracker.Clear();
        return _context;
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
