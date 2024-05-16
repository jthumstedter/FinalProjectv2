namespace MovieLibraryEntities.Models
{
    public class Occupation
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
