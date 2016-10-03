using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Scheduler.API.Core;
using Scheduler.API.ViewModels;
using Scheduler.Data;
using Scheduler.Model.Entities;

namespace Scheduler.API.Controllers
{
    [Route("api/[controller]")]
    public class SchedulesController : Controller
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IAttendeeRepository _attendeeRepository;
        private readonly IUserRepository _userRepository;
        int _page = 1;
        int _pageSize = 4;

        public SchedulesController(IScheduleRepository scheduleRepository,
                                    IAttendeeRepository attendeeRepository,
                                    IUserRepository userRepository)
        {
            _scheduleRepository = scheduleRepository;
            _attendeeRepository = attendeeRepository;
            _userRepository = userRepository;
        }

        public IActionResult Get()
        {
            var pagination = Request.Headers["Pagination"];

            if (!string.IsNullOrEmpty(pagination))
            {
                var vals = pagination.ToString().Split(',');
                int.TryParse(vals[0], out _page);
                int.TryParse(vals[1], out _pageSize);
            }

            var currentPage = _page;
            var currentPageSize = _pageSize;
            var totalSchedules = _scheduleRepository.Count();
            var totalPages = (int)Math.Ceiling((double)totalSchedules / _pageSize);

            IEnumerable<Schedule> schedules = _scheduleRepository
                .AllIncluding(s => s.Creator, s => s.Attendees)
                .OrderBy(s => s.Id)
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .ToList();

            Response.AddPagination(_page, _pageSize, totalSchedules, totalPages);

            var schedulesVm = Mapper.Map<IEnumerable<Schedule>, IEnumerable<ScheduleViewModel>>(schedules);

            return new OkObjectResult(schedulesVm);
        }

        [HttpGet("{id}", Name = "GetSchedule")]
        public IActionResult Get(int id)
        {
            var schedule = _scheduleRepository
                .GetSingle(s => s.Id == id, s => s.Creator, s => s.Attendees);

            if (schedule != null)
            {
                var scheduleVm = Mapper.Map<Schedule, ScheduleViewModel>(schedule);
                return new OkObjectResult(scheduleVm);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{id}/details", Name = "GetScheduleDetails")]
        public IActionResult GetScheduleDetails(int id)
        {
            var schedule = _scheduleRepository
                .GetSingle(s => s.Id == id, s => s.Creator, s => s.Attendees);

            if (schedule != null)
            {


                var scheduleDetailsVm = Mapper.Map<Schedule, ScheduleDetailsViewModel>(schedule);

                foreach (var attendee in schedule.Attendees)
                {
                    var userDb = _userRepository.GetSingle(attendee.UserId);
                    scheduleDetailsVm.Attendees.Add(Mapper.Map<User, UserViewModel>(userDb));
                }


                return new OkObjectResult(scheduleDetailsVm);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody]ScheduleViewModel schedule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newSchedule = Mapper.Map<ScheduleViewModel, Schedule>(schedule);
            newSchedule.DateCreated = DateTime.Now;

            _scheduleRepository.Add(newSchedule);
            _scheduleRepository.Commit();

            foreach (var userId in schedule.Attendees)
            {
                newSchedule.Attendees.Add(new Attendee { UserId = userId });
            }
            _scheduleRepository.Commit();

            schedule = Mapper.Map<Schedule, ScheduleViewModel>(newSchedule);

            var result = CreatedAtRoute("GetSchedule", new { controller = "Schedules", id = schedule.Id }, schedule);
            return result;
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]ScheduleViewModel schedule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var scheduleDb = _scheduleRepository.GetSingle(id);

            if (scheduleDb == null)
            {
                return NotFound();
            }

            scheduleDb.Title = schedule.Title;
            scheduleDb.Location = schedule.Location;
            scheduleDb.Description = schedule.Description;
            scheduleDb.Status = (ScheduleStatus)Enum.Parse(typeof(ScheduleStatus), schedule.Status);
            scheduleDb.Type = (ScheduleType)Enum.Parse(typeof(ScheduleType), schedule.Type);
            scheduleDb.TimeStart = schedule.TimeStart;
            scheduleDb.TimeEnd = schedule.TimeEnd;

            // Remove current attendees
            _attendeeRepository.DeleteWhere(a => a.ScheduleId == id);

            foreach (var userId in schedule.Attendees)
            {
                scheduleDb.Attendees.Add(new Attendee { ScheduleId = id, UserId = userId });
            }

            _scheduleRepository.Commit();

            return new NoContentResult();
        }

        [HttpDelete("{id}", Name = "RemoveSchedule")]
        public IActionResult Delete(int id)
        {
            var scheduleDb = _scheduleRepository.GetSingle(id);

            if (scheduleDb == null)
            {
                return new NotFoundResult();
            }

            _attendeeRepository.DeleteWhere(a => a.ScheduleId == id);
            _scheduleRepository.Delete(scheduleDb);

            _scheduleRepository.Commit();

            return new NoContentResult();
        }

        [HttpDelete("{id}/removeattendee/{attendee}")]
        public IActionResult Delete(int id, int attendee)
        {
            var scheduleDb = _scheduleRepository.GetSingle(id);

            if (scheduleDb == null)
            {
                return new NotFoundResult();
            }
            else
            {
                _attendeeRepository.DeleteWhere(a => a.ScheduleId == id && a.UserId == attendee);

                _attendeeRepository.Commit();

                return new NoContentResult();
            }
        }
    }
}
