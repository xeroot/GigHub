using GigHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Microsoft.AspNet.Identity;
using GigHub.Dtos;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class AttendancesController : ApiController
    {
        private ApplicationDbContext _context;

        public AttendancesController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult Attend(AttendanceDto attendanceDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Data is incorrect.");
            var attendeeId = User.Identity.GetUserId();
            if (_context.Attendances.Any(a =>
                    a.AttendeeId == attendeeId &&
                    a.GigId == attendanceDto.GigId))
                return BadRequest("The attendance already exists.");
            var attendance = new Attendance()
            {
                GigId = attendanceDto.GigId,
                AttendeeId = attendeeId
            };
            try
            {
                _context.Attendances.Add(attendance);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.InnerException);
            }
        }
    }
}
