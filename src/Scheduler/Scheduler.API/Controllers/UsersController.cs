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
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IAttendeeRepository _attendeeRepository;

        int _page = 1;
        int _pageSize = 10;
        public UsersController(IUserRepository userRepository,
                                IScheduleRepository scheduleRepository,
                                IAttendeeRepository attendeeRepository)
        {
            _userRepository = userRepository;
            _scheduleRepository = scheduleRepository;
            _attendeeRepository = attendeeRepository;
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
            var totalUsers = _userRepository.Count();
            var totalPages = (int)Math.Ceiling((double)totalUsers / _pageSize);

            IEnumerable<User> users = _userRepository
                .AllIncluding(u => u.SchedulesCreated)
                .OrderBy(u => u.Id)
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .ToList();

            var usersVm = Mapper.Map<IEnumerable<User>, IEnumerable<UserViewModel>>(users);

            Response.AddPagination(_page, _pageSize, totalUsers, totalPages);

            return new OkObjectResult(usersVm);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult Get(int id)
        {
            var user = _userRepository.GetSingle(u => u.Id == id, u => u.SchedulesCreated);

            if (user != null)
            {
                var userVm = Mapper.Map<User, UserViewModel>(user);
                return new OkObjectResult(userVm);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{id}/schedules", Name = "GetUserSchedules")]
        public IActionResult GetSchedules(int id)
        {
            var userSchedules = _scheduleRepository.FindBy(s => s.CreatorId == id);

            if (userSchedules != null)
            {
                var userSchedulesVm = Mapper.Map<IEnumerable<Schedule>, IEnumerable<ScheduleViewModel>>(userSchedules);
                return new OkObjectResult(userSchedulesVm);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody]UserViewModel user)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newUser = new User { Name = user.Name, Profession = user.Profession, Avatar = user.Avatar };

            _userRepository.Add(newUser);
            _userRepository.Commit();

            user = Mapper.Map<User, UserViewModel>(newUser);

            var result = CreatedAtRoute("GetUser", new { controller = "Users", id = user.Id }, user);
            return result;
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userDb = _userRepository.GetSingle(id);

            if (userDb == null)
            {
                return NotFound();
            }

            userDb.Name = user.Name;
            userDb.Profession = user.Profession;
            userDb.Avatar = user.Avatar;
            _userRepository.Commit();

            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var userDb = _userRepository.GetSingle(id);

            if (userDb == null)
            {
                return new NotFoundResult();
            }


            var attendees = _attendeeRepository.FindBy(a => a.UserId == id);
            var schedules = _scheduleRepository.FindBy(s => s.CreatorId == id);

            foreach (var attendee in attendees)
            {
                _attendeeRepository.Delete(attendee);
            }

            foreach (var schedule in schedules)
            {
                _attendeeRepository.DeleteWhere(a => a.ScheduleId == schedule.Id);
                _scheduleRepository.Delete(schedule);
            }

            _userRepository.Delete(userDb);

            _userRepository.Commit();

            return new NoContentResult();
        }

    }
}
