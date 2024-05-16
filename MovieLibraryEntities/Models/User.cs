using System.ComponentModel.DataAnnotations;

namespace MovieLibraryEntities.Models
{
    public class User
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Age is required")]
        [Range(1, 120, ErrorMessage = "Age must be a number between 1 and 120")]
        public long Age { get; set; }
        [Required(ErrorMessage = "Gender is required")]
        [StringLength(1, ErrorMessage = "Gender must be 1 characters")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "ZipCode is required")]
        public string ZipCode { get; set; }

        public virtual Occupation Occupation { get; set; }
        public virtual ICollection<UserMovie> UserMovies { get; set; }

        public string UserWithOccupationString(Occupation occupation)
        {
            return $"Id: {this.Id} \nAge: {this.Age} \nGender: {this.Gender} \nZip Code: {this.ZipCode}\nOccupation: {occupation.Name}";
        }

        public override string ToString()
        {
            return $"Id: {this.Id} \nAge: {this.Age} \nGender: {this.Gender} \nZipCode: {this.ZipCode}\nOccupation: {Occupation}";
        }
        public bool ValidateAge(out List<string> errors)
        {
            var context = new ValidationContext(this) { MemberName = nameof(Age) };
            errors = new List<string>();
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateProperty(Age, context, results);
            if (!isValid)
            {
                errors.AddRange(results.Select(r => r.ErrorMessage));
            }
            return isValid;
        }
        public bool ValidateGender(out List<string> errors)
        {
            var context = new ValidationContext(this) { MemberName = nameof(Gender) };
            errors = new List<string>();
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateProperty(Gender, context, results);
            if (!isValid)
            {
                errors.AddRange(results.Select(r => r.ErrorMessage));
            }
            return isValid;
        }
        public bool ValidateZipCode(out List<string> errors)
        {
            var context = new ValidationContext(this) { MemberName = nameof(ZipCode) };
            errors = new List<string>();
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateProperty(ZipCode, context, results);
            if (!isValid)
            {
                errors.AddRange(results.Select(r => r.ErrorMessage));
            }
            return isValid;
        }
    }
}
