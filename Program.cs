using System.Globalization;
using FileParser.Entities;

namespace FileParser // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var orders = new List<Order>();
            var directoryPath = PrintTitleScreen();

            try
            {
                orders = OrderProcessor.ParseFiles(Path.GetFullPath(directoryPath) ?? throw new InvalidOperationException("Please provide a valid path."));

                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Please try again....hit any key to try again.");
                Console.Read();
                //Console.Clear();
                directoryPath = PrintTitleScreen();
                orders = OrderProcessor.ParseFiles(Path.GetFullPath(directoryPath));
            }
        }

        public static string? PrintTitleScreen()
        {
            Console.Clear();
            Console.WriteLine("WELCOME TO THE FILE PARSER");
            Console.Write("Please enter the directory for the order files:  ");
            return Console.ReadLine();
        }

        //public static void PrintResults(List<Order> orders)
        //{
        //    foreach (var order in orders)
        //    {
        //        Console.WriteLine();
        //    }
        //}
    }
}