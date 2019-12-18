using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GigHub.Models
{
    public class UserNotification
    {
        [Key]
        [Column(Order = 1)]
        public string UserId { get; private set; }
        public ApplicationUser User { get; private set; }

        [Key]
        [Column(Order = 2)]
        public int NotificationId { get; private set; }
        public Notification Notification { get; private set; }

        public bool IsRead { get; private set; }

        private UserNotification() { }

        public UserNotification(ApplicationUser user, Notification notification)
        {
            if (user == null) throw new ArgumentNullException("User is null");
            if (notification == null) throw new ArgumentNullException("Notification is null");
            this.User = user;
            this.Notification = notification;
        }

        public void Read()
        {
            this.IsRead = true;
        }

    }
}