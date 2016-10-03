using System.Collections.Generic;

namespace Scheduler.Model.Entities
{
    public class User : IEntityBase
    {
        public User()
        {
            SchedulesCreated = new List<Schedule>();
            SchedulesAttended = new List<Attendee>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Profession { get; set; }
        public ICollection<Schedule> SchedulesCreated { get; set; }
        public ICollection<Attendee> SchedulesAttended { get; set; }
    }
}
