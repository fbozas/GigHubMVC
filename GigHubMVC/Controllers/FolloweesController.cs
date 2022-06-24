using GigHubMVC.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GigHubMVC.Controllers
{
    public class FolloweesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FolloweesController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Followees
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            var artists = _context.Followings
                .Where(f => f.FollowerId == userId)
                .Select(f => f.Followee)
                .ToList();

            // If I wanted the followers of  loggedin user.....
            //var followers = _context.Followings
            //    .Where(f => f.FolloweeId == userId)
            //    .Select(f => f.Follower)
            //    .ToList();

            return View(artists);
        }
    }
}