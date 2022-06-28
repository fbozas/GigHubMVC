using GigHubMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using GigHubMVC.ViewModels;
using Microsoft.AspNet.Identity;
using GigHubMVC.Repositories;

namespace GigHubMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AttendanceRepository _attendanceRepository;
        private readonly FollowingRepository _followingRepository;

        public HomeController()
        {
            _context = new ApplicationDbContext();
            _attendanceRepository = new AttendanceRepository(_context);
            _followingRepository = new FollowingRepository(_context);
        }

        public ActionResult Index(string query = null)
        {
            var upcomingGigs = _context.Gigs
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .Where(g => g.DateTime > DateTime.Now);

            if (!String.IsNullOrEmpty(query))
            {
                upcomingGigs = upcomingGigs
                    .Where(g => 
                        g.Artist.Name.Contains(query) ||
                        g.Genre.Name.Contains(query) || 
                        g.Venue.Contains(query));
            }

            var userId = User.Identity.GetUserId();

            var viewModel = new GigsViewModel()
            {
                UpcomingGigs = upcomingGigs,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Upcoming Gigs",
                SearchTerm = query,
                Attendances = _attendanceRepository.GetFutureAttendances(userId).ToLookup(a => a.GigId),
                Followings = _followingRepository.GetFollowings(userId).ToLookup(a => a.FolloweeId)
            };

            return View("Gigs",viewModel);
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