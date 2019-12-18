using GigHub.Dtos;
using GigHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Diagnostics;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class FollowingsController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public FollowingsController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult Follow(FollowingDto followingDto)
        {
            Debug.WriteLine("Entre! server.");
            if (followingDto == null || !ModelState.IsValid || string.IsNullOrEmpty(followingDto.FolloweeId))
                return BadRequest("Data is incorrect.");
            var followerId = User.Identity.GetUserId();
            if (_context.Followings.Any(f =>
                f.FolloweeId == followingDto.FolloweeId &&
                f.FollowerId == followerId))
                return BadRequest("You are already following that Artist!");
            var following = new Followings()
            {
                FollowerId = followerId,
                FolloweeId = followingDto.FolloweeId
            };
            try
            {
                _context.Followings.Add(following);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.InnerException);
            }
        }

    }
}
