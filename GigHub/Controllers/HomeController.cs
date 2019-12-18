using GigHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data.Entity;
using Microsoft.AspNet.Identity;
using GigHub.ViewModels;

namespace GigHub.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;

        public HomeController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Index(string query = null)
        {
            var userId = User.Identity.GetUserId();
            var upcomingGigs = _context.Gigs
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .Where(g => g.DateTime > DateTime.Now &&
                !g.IsCanceled); //g.ArtistId != userId && 

            if (!string.IsNullOrWhiteSpace(query))
            {
                upcomingGigs = upcomingGigs.Where(
                        g => g.Artist.UserName.Contains(query) ||
                        g.Genre.Name.Contains(query) ||
                        g.Venue.Contains(query)
                    );
            }

            var model = new GigViewModel()
            {
                Heading = "Upcoming Gigs",
                UpcominGigs = upcomingGigs,
                ShowActions = User.Identity.IsAuthenticated,
                SearchTerm = query
            };
            return View("Gigs", model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}