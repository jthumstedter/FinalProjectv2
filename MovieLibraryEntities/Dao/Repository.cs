using Microsoft.EntityFrameworkCore;
using MovieLibraryEntities.Context;
using MovieLibraryEntities.Models;

namespace MovieLibraryEntities.Dao
{
    public class Repository : IRepository, IDisposable
    {
        private readonly IDbContextFactory<MovieContext> _contextFactory;
        private readonly MovieContext _context;
        public Repository()
        {
            _context = new MovieContext();
        }

        public Repository(IDbContextFactory<MovieContext> contextFactory)
        {
            _contextFactory = contextFactory;
            _context = _contextFactory.CreateDbContext();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IEnumerable<Movie> GetAllMovies()
        {
            return _context.Movies.ToList();
        }
        public IEnumerable<Genre> GetAllGenres()
        {
            return _context.Genres.ToList();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public IEnumerable<Occupation> GetOccupations()
        {
            return _context.Occupations.ToList();
        }

        public IEnumerable<Movie> Search(string searchString)
        {
            var allMovies = _context.Movies;
            var listOfMovies = allMovies.ToList();
            var temp = listOfMovies.Where(x => x.Title.Contains(searchString, StringComparison.CurrentCultureIgnoreCase));

            return temp;
        }

        public IEnumerable<UserMovie> GetUserMoviesWithUserAndOccupation()
        {
            return _context.UserMovies
                .Include(um => um.User)
                .ThenInclude(u => u.Occupation)
                .Include(um => um.Movie)
                .ToList();
        }

        public IEnumerable<MovieGenre> GetMovieGenres()
        {
            // Fetches all MovieGenre data from the database, including the related Movie and Genre data
            return _context.MovieGenres
                .Include(mg => mg.Movie)
                .Include(mg => mg.Genre)
                .ToList();
        }
    }
}
