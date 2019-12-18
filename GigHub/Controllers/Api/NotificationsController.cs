using GigHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Microsoft.AspNet.Identity;
using System.Data.Entity;
using GigHub.Dtos;
using AutoMapper;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class NotificationsController : ApiController
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private readonly ApplicationDbContext _context;

        public NotificationsController()
        {
            _context = new ApplicationDbContext();
        }

        public IEnumerable<NotificationDto> GetNewNotifications()
        {
            var userId = User.Identity.GetUserId();
            var notifications = _context.UserNotifications
                .Where(un => un.UserId == userId && !un.IsRead)
                .Select(un => un.Notification)
                .Include(n => n.Gig.Artist)
                .Include(n => n.Gig.Genre)
                .ToList();
            return notifications.Select(Mapper.Map<Notification, NotificationDto>);
        }

        [HttpPost]
        public IHttpActionResult MarkAsRead()
        {
            var userId = User.Identity.GetUserId();
            try
            {
                var notifications = _context.UserNotifications
                    .Where(un => un.UserId == userId && !un.IsRead);

                foreach (var notification in notifications)
                    notification.Read();

                _context.SaveChanges();
                log.Info("User {0} has read all his notifications.", userId);
                return Ok();
            }
            catch (Exception ex)
            {
                log.Error(ex, "User {0} coun't read all his notifications cause an error: {1}", userId, ex.InnerException.Message);
                return InternalServerError(ex);
            }
        }


    }
}
