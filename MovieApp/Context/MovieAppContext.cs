using Microsoft.IdentityModel.Tokens;
using MovieLibraryEntities.Context;
using MovieLibraryEntities.Dao;
using MovieLibraryEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Context
{
    public class MovieAppContext
    {
        public Movie SearchMovieFromDatabase()
        {
            using (var repo = new Repository())
            {
                string searchedMovie;
                List<Movie> selectedMovie;
                while (true)
                {
                    while (true)
                    {
                        Console.Write("Enter Movie Name: ");
                        searchedMovie = Console.ReadLine();
                        if (searchedMovie.IsNullOrEmpty())
                        {
                            Console.WriteLine("Please enter the name of the movie");
                        }
                        else
                        {
                            break;
                        }

                    }
                    //selectedMovie = repo.GetAllMovies().FirstOrDefault(x => x.Title.ToUpper() == searchedMovie.ToUpper());
                    selectedMovie = repo.Search(searchedMovie).ToList();
                    if (selectedMovie.Count == 0)
                    {
                        Console.WriteLine("Please enter a movie that is in the database");
                    }
                    else
                    {
                        return selectedMovie.First();
                    }

                }
            }
        }
        public void SearchMovie()
        {
            using (var repo = new Repository())
            {
                var selectedMovie = SearchMovieFromDatabase();
                var genreList = repo.GetMovieGenres().Where(x => x.Movie.Id == selectedMovie.Id).Select(g => g.Genre);
                Console.WriteLine(selectedMovie.MovieWithGenresString(genreList));
                Console.WriteLine();
            }
        }
        public void ListMovie()
        {
            using (var repo = new Repository())
            {
                var movieList = repo.GetAllMovies();
                var genreList = repo.GetMovieGenres();
                foreach (var movie in movieList)
                {
                    var genreListForThisMovie = genreList.Where(x => x.Movie.Id == movie.Id).Select(g => g.Genre);
                    Console.WriteLine(movie.MovieWithGenresString(genreListForThisMovie));
                    Console.WriteLine();
                }
            }
        }
        public string MakeMovieTitle(string prompt)
        {
            var tempMovie = new Movie();
            List<string> titleErrors;
            while (true)
            {
                Console.Write(prompt);
                tempMovie.Title = Console.ReadLine();

                if (tempMovie.ValidateTitle(out titleErrors))
                {
                    break;
                }
                DisplayErrors(titleErrors);
            }
            return tempMovie.Title;
        }
        public DateTime MakeMovieReleaseDate(string prompt)
        {
            DateTime releaseDate = new DateTime();
            while (true)
            {
                Console.Write(prompt);
                bool validDate = DateTime.TryParse(Console.ReadLine(), out releaseDate);
                if (validDate)
                {
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Please try again.");
                    Console.ResetColor();
                }
            }
            return releaseDate;
        }

        public void AddMovie()
        {
            var movie = new Movie();
            movie.Title = MakeMovieTitle("Enter movie title: ");
            movie.ReleaseDate = MakeMovieReleaseDate($"Enter {movie.Title} release date (MM/DD/YYYY): ");
            using (var context = new MovieContext())
            {
                context.Movies.Add(movie);
                context.SaveChanges();
                var newMovie = context.Movies.FirstOrDefault(m => m.Title.Equals(movie.Title));
                var exitLoop = false;
                while (!exitLoop)
                {
                    Console.Write("Enter a genre: ");
                    var inputGenre = Console.ReadLine();
                    var selectedGenre = context.Genres.FirstOrDefault(x => x.Name == inputGenre);
                    if (selectedGenre != null)
                    {
                        var createdMovieGenre = new MovieGenre();
                        createdMovieGenre.Movie = newMovie;
                        createdMovieGenre.Genre = selectedGenre;
                        context.MovieGenres.Add(createdMovieGenre);
                        context.SaveChanges();
                        Console.WriteLine("Do you want another genre (y/n)?: ");
                        var input = Console.ReadLine();
                        exitLoop = input == "N" || input == "n";
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid input. Please try again.");
                        Console.ResetColor();
                    }
                }
            }
            Console.WriteLine($"{movie.Title} has been added to the database. Press enter key to exit.");
            Console.ReadLine();
        }

        private static void DisplayErrors(List<string> errors)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid input. Please try again.");
            foreach (var error in errors)
            {
                Console.WriteLine(error);
            }

            Console.ResetColor();
        }

        public void UpdateMovie()
        {
            var selectedMovie = SearchMovieFromDatabase();

            var updatedName = MakeMovieTitle("Enter the updated movie title: ");

            var updatedReleaseDate = MakeMovieReleaseDate($"Enter {updatedName} updated release date (MM/DD/YYYY): ");
            
            using (var context = new MovieContext())
            {
                var updateMovie = context.Movies.FirstOrDefault(x => x.Title == selectedMovie.Title);
                updateMovie.Title = updatedName;
                updateMovie.ReleaseDate = updatedReleaseDate;
                context.Movies.Update(updateMovie);
                context.SaveChanges();
            }
            
        }

        public void ListUsers()
        {
            using (var repo = new Repository())
            {
                var userList = repo.GetAllUsers();
                var occupationList = repo.GetOccupations();
                foreach (var user in userList)
                {
                    var occupationForThisUser = occupationList.Where(x => x.Id == user.Occupation.Id).First();
                    Console.WriteLine(user.UserWithOccupationString(occupationForThisUser));
                    Console.WriteLine();
                }
            }
        }

        public void AddUser()
        {
            var user = new User();
            List<string> ageErrors;
            while (true)
            {
                Console.Write("Enter user age (Positive number between 0-120): ");
                if (!long.TryParse(Console.ReadLine(), out var age))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid age format. Please enter a valid number.");
                    Console.ResetColor();
                    continue;
                }

                user.Age = age;

                if (user.ValidateAge(out ageErrors))
                {
                    break;
                }
                DisplayErrors(ageErrors);
            }
            List<string> genderErrors;
            while (true)
            {
                Console.Write("Enter user gender (One character long): ");
                user.Gender = Console.ReadLine();

                if (user.ValidateGender(out genderErrors))
                {
                    break;
                }
                DisplayErrors(genderErrors);
            }
            List<string> zipCodeErrors;
            while (true)
            {
                Console.Write("Enter user ZIP code: ");
                user.ZipCode = Console.ReadLine();

                if (user.ValidateZipCode(out zipCodeErrors))
                {
                    break;
                }
                DisplayErrors(zipCodeErrors);
            }
            using (var repo = new Repository())
            using (var context = new MovieContext())
            {
                while (true)
                {
                    Console.Write("Enter users occupation: ");
                    var inputOccupation = Console.ReadLine();
                    var selectedOccupation = context.Occupations.FirstOrDefault(x => x.Name == inputOccupation);
                    if (selectedOccupation != null)
                    {
                        user.Occupation = selectedOccupation;
                        context.Users.Add(user);
                        context.SaveChanges();
                        break;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid input. Please try again.");
                        Console.ResetColor();
                    }
                }
                var newUser = repo.GetAllUsers().Last();
                var occupationList = repo.GetOccupations();
                var occupationForThisUser = occupationList.Where(x => x.Id == user.Occupation.Id).First();
                Console.WriteLine(user.UserWithOccupationString(occupationForThisUser));
                Console.Write("Press enter key to exit.");
                Console.ReadLine();
            }
        }

    }
}
