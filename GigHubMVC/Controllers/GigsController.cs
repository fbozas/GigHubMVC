using GigHubMVC.Models;
using GigHubMVC.Persistence;
using GigHubMVC.Repositories;
using GigHubMVC.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GigHubMVC.Controllers
{
    public class GigsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AttendanceRepository _attendanceRepository;
        private readonly GigRepository _gigRepository;
        private readonly FollowingRepository _followingRepository;
        private readonly GenreRepository _genreRepository;
        private readonly UnitOfWork _unitOfWork;

        public GigsController()
        {
            _context = new ApplicationDbContext();
            _attendanceRepository = new AttendanceRepository(_context);
            _gigRepository = new GigRepository(_context);
            _followingRepository = new FollowingRepository(_context);
            _genreRepository = new GenreRepository(_context);
            _unitOfWork = new UnitOfWork(_context);
        }

        [Authorize]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();
            var gigs = _gigRepository.GetUpcomingGigsByArtist(userId);

            return View(gigs);
        }

        public ActionResult Details(int id)
        {
            var gig = _gigRepository.GetGig(id);

            if(gig == null)
                return HttpNotFound();

            var viewModel = new GigDetailsViewModel() { Gig = gig };

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();

                //viewModel.IsAttending = _context.Attendances
                //    .Any(a => a.GigId == gig.Id && a.AttendeeId == userId);
                viewModel.IsAttending = _attendanceRepository.GetAttendance(gig.Id, userId) != null;

                //viewModel.IsFollowing = _context.Followings
                //    .Any(f => f.FolloweeId == gig.ArtistId && f.FollowerId == userId);
                viewModel.IsFollowing = _followingRepository.GetFollowing(userId, gig.ArtistId) != null;

            }

            return View("Details", viewModel);
        }
 

        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();

            var viewModel = new GigsViewModel()
            {
                UpcomingGigs = _gigRepository.GetGigsUserAttending(userId),
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Gigs I 'm attending",
                Attendances = _attendanceRepository.GetFutureAttendances(userId).ToLookup(a => a.GigId),
                Followings = _followingRepository.GetFollowings(userId).ToLookup(a => a.FolloweeId)
            };

            return View("Gigs",viewModel);
        }

        [HttpPost]
        public ActionResult Search(GigsViewModel viewModel)
        {
            return RedirectToAction("Index", "Home", new { query = viewModel.SearchTerm });
        }

        // GET: Gigs
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new GigFormViewModel()
            {
                Genres = _genreRepository.GetGenres(),
                Heading = "Add a Gig"
            };

            return View("GigForm", viewModel);
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
                Venue = viewModel.Venue,
                GenreId = viewModel.Genre
            };

            _gigRepository.Add(gig);
            _unitOfWork.Complete();

            return RedirectToAction("Mine", "Gigs");
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var gig = _gigRepository.GetGig(id);

            if (gig == null)
                return HttpNotFound();

            if (gig.ArtistId != User.Identity.GetUserId())
                return new HttpUnauthorizedResult();

            var viewModel = new GigFormViewModel()
            {
                Id = gig.Id,
                Heading = "Edit a Gig",
                Genres = _genreRepository.GetGenres(),
                Date = gig.DateTime.ToString("d MMM yyyy"),
                Time = gig.DateTime.ToString("HH:mm"),
                Genre = gig.GenreId,
                Venue = gig.Venue
            };

            return View("GigForm", viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                return View("GigForm", viewModel);
            }

            var gig = _gigRepository.GetGigWithAttendees(viewModel.Id);

            if (gig == null)
                return HttpNotFound();

            if (gig.ArtistId != User.Identity.GetUserId())
                return new HttpUnauthorizedResult();

            gig.Venue = viewModel.Venue;
            gig.DateTime = viewModel.GetDateTime();
            gig.GenreId = viewModel.Genre;

            _unitOfWork.Complete();

            return RedirectToAction("Mine", "Gigs");
        }


    }
}