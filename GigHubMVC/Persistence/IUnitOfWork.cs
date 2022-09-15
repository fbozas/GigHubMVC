using GigHubMVC.Repositories;

namespace GigHubMVC.Persistence
{
    public interface IUnitOfWork
    {
        IAttendanceRepository Attendances { get; }
        IFollowingRepository Followings { get; }
        IGenreRepository Genres { get; }
        IGigRepository Gigs { get; }

        IApplicationUserRepository Users { get; }

        void Complete();
    }
}