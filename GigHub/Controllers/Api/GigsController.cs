using GigHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class GigsController : ApiController
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private readonly ApplicationDbContext _context;

        public GigsController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpDelete]
        public IHttpActionResult Cancel(int id)
        {
            var userId = User.Identity.GetUserId();
            try
            {
                var gig = _context.Gigs
                    .Include(g => g.Attendances.Select(a => a.Attendee))
                    .Single(g => g.Id == id && g.ArtistId == userId);
                if (gig.IsCanceled) return NotFound();

                gig.Cancel();

                _context.SaveChanges();
                log.Info("Gig '{0}' canceled.", id);
                return Ok();
            }
            catch (Exception ex)
            {
                log.Error(ex, "Can't cancel gig '{0}'. Gig not found or not owned by user '{1}'.", id, userId);
                return BadRequest("You can't cancel this Gig.");
            }
        }
    }
}
