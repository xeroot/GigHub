using System;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GigHub.Models
{
    public class ApplicationUser : IdentityUser
    {
        /*[Required]
        [StringLength(100)]
        public string Email { get; set; }*/

        public ICollection<Followings> Followers { get; set; }
        public ICollection<Followings> Followees { get; set; }
        public ICollection<UserNotification> UserNotitifications { get; set; }

        public ApplicationUser()
        {
            Followers = new Collection<Followings>();
            Followees = new Collection<Followings>();
            UserNotitifications = new Collection<UserNotification>();
        }

        /*[Required]
        [StringLength(255)]
        public override string UserName
        {
            get
            {
                return base.UserName;
            }
            set
            {
                base.UserName = value;
            }
        }*/

        public void Notify(Notification notification)
        {
            var userNotification = new UserNotification(this, notification);
            UserNotitifications.Add(userNotification);
        }
    }
}