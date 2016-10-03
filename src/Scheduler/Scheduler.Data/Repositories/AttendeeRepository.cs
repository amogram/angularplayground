using Scheduler.Model.Entities;

namespace Scheduler.Data.Repositories
{
    public class AttendeeRepository : EntityBaseRepository<Attendee>, IAttendeeRepository
    {
        public AttendeeRepository(SchedulerContext context)
            : base(context)
        { }
    }
}
