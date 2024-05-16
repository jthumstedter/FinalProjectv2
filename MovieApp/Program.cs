
using MovieApp.Context;
using MovieLibraryEntities.Dao;

public class Program
{
    static public void Main(string[] args)
    {
        MovieAppContext movieAppContext = new MovieAppContext();
        bool exitLoop = true;
        do
        {
            Console.WriteLine("1) Search Movie");
            Console.WriteLine("2) Add Movie");
            Console.WriteLine("3) List Movies");
            Console.WriteLine("4) Update Movie");
            Console.WriteLine("5) List Users");
            Console.WriteLine("6) Add User");
            Console.Write("Enter your choice (-1 to exit): ");
            var input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    movieAppContext.SearchMovie();
                    break;
                case "2":
                    movieAppContext.AddMovie();
                    break;
                case "3":
                    movieAppContext.ListMovie();
                    break;
                case "4":
                    movieAppContext.UpdateMovie();
                    break;
                case "5":
                    movieAppContext.ListUsers();
                    break;
                case "6":
                    movieAppContext.AddUser();
                    break;
                case "-1":
                    exitLoop = false;
                    break;
            }
        } while (exitLoop);
    }
}




