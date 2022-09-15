using GigHubMVC.Models;
using GigHubMVC.Persistence;
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
        private readonly IUnitOfWork _unitOfWork;

        public FolloweesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // GET: Followees
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            var artists = _unitOfWork.Users.GetArtistsFollowedBy(userId);

            // If I wanted the followers of  loggedin user.....
            //var followers = _context.Followings
            //    .Where(f => f.FolloweeId == userId)
            //    .Select(f => f.Follower)
            //    .ToList();

            return View(artists);
        }
    }
}