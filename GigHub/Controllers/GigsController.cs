using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Diagnostics;

using System.Data.Entity;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private readonly ApplicationDbContext _context;

        public GigsController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: /Gigs/
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new GigFormViewModel()
            {
                Heading = "Create Gig",
                Genres = _context.Genres.ToList()
            };
            return View("GigForm", viewModel);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            try
            {
                var gig = _context.Gigs
                    .Single(g => g.Id == id && g.ArtistId == userId);
                var viewModel = new GigFormViewModel()
                {
                    Id = gig.Id,
                    Heading = "Edit Gig",
                    Venue = gig.Venue,
                    Genre = gig.GenreId,
                    Date = gig.DateTime.ToString("d MMM yyyy"),
                    Time = gig.DateTime.ToString("HH:mm"),
                    Genres = _context.Genres.ToList()
                };
                return View("GigForm", viewModel);
            }
            catch (Exception ex)
            {
                log.Info(ex, "Gig not found or Gig not owned by User '{0}'.", userId);
                return RedirectToAction("Mine");
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                log.Warn("El modelo no es válido.");
                viewModel.Genres = _context.Genres.ToList();
                return View("GigForm", viewModel);
            }
            var userId = User.Identity.GetUserId();
            try
            {
                var gig = _context.Gigs
                    .Include(g => g.Attendances.Select(a => a.Attendee))
                    .Single(g => g.Id == viewModel.Id && g.ArtistId == userId);
                gig.Modify(viewModel.Venue, viewModel.GetDateTime(), viewModel.Genre);
                _context.SaveChanges();
                return RedirectToAction("Mine");
            }
            catch (Exception ex)
            {
                log.Error(ex, "The gig couldn't be found or is not owned by the user.");
                viewModel.Genres = _context.Genres.ToList();
                return View("GigForm", viewModel);
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                return View("GigForm", viewModel);
            }

            var gig = new Gig()
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                GenreId = viewModel.Genre,
                Venue = viewModel.Venue
            };

            _context.Gigs.Add(gig);
            _context.SaveChanges();
            return RedirectToAction("Mine");
        }

        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();
            var gigs = _context.Attendances
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Gig)
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .ToList();
            var model = new GigViewModel()
            {
                Heading = "Gigs I'm atteding.",
                UpcominGigs = gigs,
                ShowActions = User.Identity.IsAuthenticated
            };
            return View("Gigs", model);
        }

        [Authorize]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();
            var gigs = _context.Gigs
                .Where(g =>
                    g.ArtistId == userId &&
                    g.DateTime > DateTime.Now &&
                    !g.IsCanceled)
                .Include(g => g.Genre)
                .ToList();
            return View(gigs);
        }

        [HttpPost]
        public ActionResult Search(GigViewModel viewModel)
        {
            return RedirectToAction("Index", "Home", new { query = viewModel.SearchTerm });
        }

    }
}