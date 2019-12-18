using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GigHub.Models
{
    public class Notification
    {
        public int Id { get; private set; }
        public DateTime DateTime { get; private set; }
        public DateTime? OriginalDateTime { get; private set; }
        public NotificationType Type { get; private set; }

        [StringLength(255)]
        public string OriginalVenue { get; private set; }

        [Required]
        public int GigId { get; private set; }
        public Gig Gig { get; private set; }

        public Notification() { }

        private Notification(Models.Gig gig, Models.NotificationType notificationType)
        {
            if (gig == null) throw new ArgumentNullException("Gig is null.");
            this.DateTime = DateTime.Now;
            this.Gig = gig;
            this.Type = notificationType;
        }

        public static Notification GigCreated(Gig gig)
        {
            return new Notification(gig, NotificationType.GigCreated);
        }

        public static Notification GigUpdated(Gig gig, string venue, DateTime dateTime)
        {
            var notification = new Notification(gig, NotificationType.GigUpdated);
            notification.OriginalDateTime = dateTime;
            notification.OriginalVenue = venue;
            return notification;
        }

        public static Notification GigCanceled(Gig gig)
        {
            return new Notification(gig, NotificationType.GigCanceled);
        }

    }
}