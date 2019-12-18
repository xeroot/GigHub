using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace GigHub.Models
{
    public class Gig
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsCanceled { get; private set; }

        [Required]
        [StringLength(255)]
        public string Venue { get; set; }

        [Required]
        public byte GenreId { get; set; }
        public Genre Genre { get; set; }

        [Required]
        public string ArtistId { get; set; }
        public ApplicationUser Artist { get; set; }

        public ICollection<Attendance> Attendances { get; private set; }

        public Gig()
        {
            Attendances = new Collection<Attendance>();
        }

        public void Cancel()
        {
            IsCanceled = true;
            var notification = Notification.GigCanceled(this);
            foreach (var attendee in Attendances.Select(a => a.Attendee))
                attendee.Notify(notification);
        }

        public void Modify(string venue, DateTime dateTime, byte genreId)
        {
            if (venue == null) throw new ArgumentNullException("Venue argument is null.");
            var notification = Notification.GigUpdated(this, Venue, DateTime);
            GenreId = genreId;
            if (dateTime != DateTime || venue != Venue)
            {
                DateTime = dateTime;
                Venue = venue;
                foreach (var attendee in Attendances.Select(a => a.Attendee))
                    attendee.Notify(notification);
            }
        }
    }
}