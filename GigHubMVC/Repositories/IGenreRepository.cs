using GigHubMVC.Models;
using System.Collections.Generic;

namespace GigHubMVC.Repositories
{
    public interface IGenreRepository
    {
        IEnumerable<Genre> GetGenres();
    }
}