using System;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace MoviePlex_Project
{
    class Program
    {      
        static void Main(string[] args)
        {
            Movie movie = new Movie();
        }
    }

    class Movie
    {
        private readonly string admin_password = "21232f297a57a5a743894a0e4a801fc3";
        private int countPass = 5;

        private Dictionary<string, string> dictMovies = new Dictionary<string, string>();
        private int totalMovies = 0;
        string movieName, movieRating;
        private string[] ageRestrictions = new string[5] { "G", "PG", "PG-13", "R", "NC-17" };

        ArrayList seats = new ArrayList();

        public Movie()
        {
            WelcomeMsg();
            Choice();
        }

        public void Administrator()
        {
            CheckPass();
        }

        public void Guest()
        {
            Console.Clear();
            WelcomeMsg();
            DisplayMovies();            
        }


        public void Choice()
        {
            string choice;

            bool temp = true;
            while (temp == true)
            {
                Console.Write("\nPlease Select from the following options : \n1: Administrator\n2: Guests\n0: Exit\n\nSelection: ");
                choice = Console.ReadLine();
                int c;
                if (int.TryParse(choice, out _))
                {
                    c = int.Parse(choice);
                    
                    switch (c)
                    {
                        case 0:
                            temp = false;
                            Environment.Exit(0);
                            break;
                        case 1:
                            Administrator();
                            break;
                        case 2:
                            Guest();
                            break;
                        default:
                            Console.WriteLine("Invalid Choice !! Try Again or Quit !!!");
                            break;
                    }
                    c = 0;
                }
                else
                {
                    Console.WriteLine("Invalid Form of Choice !!");
                    Choice();
                }                
            }
        }

        public void WelcomeMsg()
        {
            for (int i = 0; i < 40; i++)
            {
                Console.Write(" ");
            }
            for (int i = 0; i < 34; i++)
            {
                Console.Write("*");
            }
            Console.WriteLine();
            for (int i = 0; i < 40; i++)
            {
                Console.Write(" ");
            }
            for (int i = 0; i < 3; i++)
            {
                Console.Write("*");
            }
            Console.Write("Welcome to MoviePlex Theatre");
            for (int i = 0; i < 3; i++)
            {
                Console.Write("*");
            }
            Console.WriteLine();
            for (int i = 0; i < 40; i++)
            {
                Console.Write(" ");
            }
            for (int i = 0; i < 34; i++)
            {
                Console.Write("*");
            }
            Console.WriteLine();
        }

        private string Encrypt(string pass)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(pass));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

        private void CheckPass()
        {
            if (countPass == 0)
            {
                countPass = 5;
                Console.Clear();
                WelcomeMsg();
                Choice();
            }
            else
            {
                string pass = "";
                Console.Write("\nEnter your password: ");
                ConsoleKeyInfo key;

                do
                {
                    key = Console.ReadKey(true);

                    if (key.Key == ConsoleKey.B)
                    {
                        countPass = 5;
                        Console.Clear();
                        WelcomeMsg();
                        Choice();
                        break;
                    }

                    if (key.Key != ConsoleKey.Backspace)
                    {
                        pass += key.KeyChar;
                        Console.Write("*");
                    }
                    else
                    {
                        pass = pass.Remove(pass.Length - 1);
                        Console.Write("\b \b");
                    }
                } while (key.Key != ConsoleKey.Enter);
                pass = pass.Remove(pass.Length - 1);
                pass = Encrypt(pass);
                if (admin_password != pass)
                {
                    countPass--;
                    Console.WriteLine("\n\nAccess Denied ....!!!!");
                    Console.WriteLine("\n{0} attempts left OR press B to go back to previous Menu.", countPass);
                    CheckPass();
                }
                else
                {
                    Console.Clear();
                    MoviesToday();
                }
            }
        }

        private void MoviesToday()
        {
            Console.WriteLine("Welcome MoviePlex Administrator\n\n");

            string[] positions = new string[10] { "First", "Second", "Third", "Fourth", "Fifth", "Sixth", "Seventh", "Eighth", "Nineth", "Tenth" };

            Console.Write("How many movies playing today ? : ");
            totalMovies = Convert.ToInt32(Console.ReadLine());            

            for (int i = 0; i < totalMovies; i++)
            {
                Console.Write("\n\nPlease enter the {0} Movie's Name : ", positions[i]);
                movieName = Console.ReadLine();

            InvalidAgeRestrictions:
                Console.Write("Please enter the Age Limit or Rating for the {0} movie : ", positions[i]);
                movieRating = Console.ReadLine();
                
                if (!int.TryParse(movieRating, out _))
                {
                    if (!(Array.Exists(ageRestrictions, element => element == movieRating)))
                    {
                        goto InvalidAgeRestrictions;
                    }
                }                
                dictMovies.Add(movieName, movieRating);
            }

            Console.WriteLine("\n\n\n");

            int j = 1;

            for (int i = 0; i < totalMovies; i++)
            {
                Console.WriteLine("{0}. {1} ({2})", (j), dictMovies.Keys.ElementAt(i), dictMovies.Values.ElementAt(i));
                j++;
            }

        CheckAgain:
            Console.Write("\nYour Movies Playing Today are listed above. Are you satisfied? (Y/N)");
            ConsoleKeyInfo ans = Console.ReadKey(true);
        
            if (ans.Key == ConsoleKey.Y)
            {
                Console.Clear();
                WelcomeMsg();
                Choice();
            }
            else if (ans.Key == ConsoleKey.N)
            {
                Console.Clear();
                MoviesToday();
            }
            else
            {
                Console.WriteLine("Invalid option !!!");
                goto CheckAgain;
            }
        }

        private void DisplayMovies()
        {
            if(totalMovies == 0)
            {
                Console.WriteLine("Theatre is under maintenance. \nPlease Contact Administrator for further details/querires. \nPress any key to go to Start Page.");
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                Console.Clear();
                WelcomeMsg();
                Choice();
            }
            Console.WriteLine("There are {0} movies playing today. Please choose from the following movies : ", totalMovies);
            int j = 1;


            for(int i = 0; i < totalMovies; i++)
            {
                Console.WriteLine("\t{0}. {1} ({2})", j, dictMovies.Keys.ElementAt(i), dictMovies.Values.ElementAt(i));                
                j++;
            }

            SelectMovieAgain:
            Console.Write("\nWhich movie would you like to watch : ");
            int movieChoice = Convert.ToInt32(Console.ReadLine());

            if(movieChoice >= j || movieChoice <= 0)
            {
                Console.WriteLine("Invalid Movie !!!!");
                goto SelectMovieAgain;
            }            
            
            Console.Write("Please enter your age for verification : ");
            int ageVerify = Convert.ToInt32(Console.ReadLine());

            if (ageVerify < 0 && !AgeVerification(ageVerify, dictMovies.Values.ElementAt((movieChoice - 1))))
            {                
                Console.WriteLine("\nInvalid Age to watch this MOVIE !!!!\t\tPlease come with Parents !!!!\n");
                goto SelectMovieAgain;
            }
            
            SelectSeats();

        CheckMenu:            
            Console.WriteLine("Press M to go back to guest menu.");
            Console.WriteLine("Press S to go back to start page.");

            ConsoleKeyInfo menu = Console.ReadKey(true);

            if(menu.Key == ConsoleKey.M)
            {
                Guest();
            }
            else if(menu.Key == ConsoleKey.S)
            {
                Console.Clear();
                WelcomeMsg();
                Choice();
            }
            else
            {
                goto CheckMenu;   
            }
        }

        private static bool AgeVerification(int age, string rating)
        {
            if(int.TryParse(rating, out _))
            {
                int numAge = int.Parse(rating);
                if(age >= numAge)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if(rating == "G")
            {
                return true;
            }
            else if(rating == "PG" && age >= 10)
            {
                return true;
            }
            else if(rating == "PG-13" && age >= 13)
            {
                return true;
            }
            else if(rating == "R" && age >= 15)
            {
                return true;
            }
            else if(rating == "NC-17" && age >= 17)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SelectSeats()
        {
            LoadSeats();            

            SelectSeatsAgain:
            Console.Write("\nSelect Row and Number of Seat (e.g: B1,E5) : ");
            string seat = Console.ReadLine();

            if(seat.Length > 3 || seat.Length <= 1)
            {
                Console.WriteLine("Invalid Seat Number !!");
                goto SelectSeatsAgain;
            }
            else if (!seats.Contains(seat))
            {
                Console.WriteLine("Invalid Seat Number OR Seat has been booked !!");
                goto SelectSeatsAgain;
            }
            else
            {
                Console.WriteLine("Booking Confirm : " +seat);
                Console.WriteLine("\nEnjoy The Movie !\n\n");
                seats.Remove(seat);
            }
        }

        private void LoadSeats()
        {
            Console.WriteLine("\n\n\t\t\t\t**************Screen**************\n\n");
            
            int j = 65, k = 1;
            for (int i = 0; i < 280; i++)
            {
                if (k < 20)
                {
                    seats.Add((char)j + k.ToString());
                    k++;
                }
                else
                {                    
                    seats.Add((char)j + k.ToString());
                    k = 1;
                    j++;
                }                
            }
            
            for (int i = 78; i >= 65; i--)
            {
                Console.WriteLine("\t\t| * * * * <={0}\t{1}=> * * * * * * * * * * * * <={2}\t{3}=> * * * * |", (char)i + "1-4", (char)i + "5-10", (char)i + "11-16", (char)i + "17-20");
            }                       
        }
    }
}