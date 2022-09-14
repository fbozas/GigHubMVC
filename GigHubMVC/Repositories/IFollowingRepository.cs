using GigHubMVC.Models;
using System.Collections.Generic;

namespace GigHubMVC.Repositories
{
    public interface IFollowingRepository
    {
        void Add(Following following);
        Following GetFollowing(string followerId, string followeeId);
        IEnumerable<Following> GetFollowings(string userId);
        void Remove(Following following);
    }
}