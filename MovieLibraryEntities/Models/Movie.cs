using System.ComponentModel.DataAnnotations;

namespace MovieLibraryEntities.Models
{
    public class Movie
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Movie name is required")]
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }


        public virtual ICollection<MovieGenre> MovieGenres { get; set; }
        public virtual ICollection<UserMovie> UserMovies { get; set; }

        public string MovieWithGenresString(IEnumerable<Genre> genres)
        {
            var genresString = "Genres: ";
            foreach (var genre in genres)
            {
                genresString += genre + " ";
            }
            return $"Id: {this.Id} \nTitle: {this.Title} \nRelease Date: {this.ReleaseDate:MM}/{this.ReleaseDate:dd}/{this.ReleaseDate:yyyy}\n{genresString}";
        }

        public override string ToString()
        {
            return $"Id: {this.Id} \nTitle: {this.Title} \nRelease Date: {this.ReleaseDate:MM}/{this.ReleaseDate:dd}/{this.ReleaseDate:yyyy}";
        }

        public bool ValidateTitle(out List<string> errors)
        {
            var context = new ValidationContext(this) { MemberName = nameof(Title) };
            errors = new List<string>();
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateProperty(Title, context, results);
            if (!isValid)
            {
                errors.AddRange(results.Select(r => r.ErrorMessage));
            }
            return isValid;
        }
    }
}
