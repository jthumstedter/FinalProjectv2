using MovieLibraryEntities.Models;

namespace MovieLibraryEntities.Dao
{
    public interface IRepository
    {
        IEnumerable<Movie> GetAllMovies();
        IEnumerable<Movie> Search(string searchString);
    }
}
