using Scheduler.Model.Entities;

namespace Scheduler.Data.Repositories
{
    public class ScheduleRepository : EntityBaseRepository<Schedule>, IScheduleRepository
    {
        public ScheduleRepository(SchedulerContext context)
            : base(context)
        { }
    }
}
