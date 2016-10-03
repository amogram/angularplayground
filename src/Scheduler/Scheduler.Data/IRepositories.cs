using Scheduler.Model.Entities;

namespace Scheduler.Data
{
    public interface IScheduleRepository : IEntityBaseRepository<Schedule> { }

    public interface IUserRepository : IEntityBaseRepository<User> { }

    public interface IAttendeeRepository : IEntityBaseRepository<Attendee> { }
}
