using GigHubMVC.Models;
using System.Collections.Generic;

namespace GigHubMVC.Repositories
{
    public interface IAttendanceRepository
    {
        void Add(Attendance attendance);
        Attendance GetAttendance(int gigId, string userId);
        IEnumerable<Attendance> GetFutureAttendances(string userId);
        void Remove(Attendance attendance);
    }
}