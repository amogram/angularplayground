using Scheduler.Model.Entities;

namespace Scheduler.Data.Repositories
{
    public class UserRepository : EntityBaseRepository<User>, IUserRepository
    {
        public UserRepository(SchedulerContext context)
            : base(context)
        { }
    }
}
