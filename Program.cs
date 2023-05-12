using System.Globalization;
using FileParser.Entities;

namespace FileParser // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            List<Order> orders = new List<Order>();
            string directoryPath = PrintTitleScreen();
            OrderProcessor fileProcessor = new OrderProcessor(directoryPath);

            while (true)
            {
                try
                {
                    orders = fileProcessor.ParseFiles(Path.GetFullPath(directoryPath) ?? throw new InvalidOperationException("Please provide a valid path."));
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Please try again....hit any key to try again.");
                    Console.ReadLine();

                }
            }
        }

        public static string? PrintTitleScreen()
        {
            Console.Clear();
            Console.WriteLine("WELCOME TO THE FILE PARSER");
            Console.Write("Please enter the directory for the order files:  ");
            return Console.ReadLine();
        }
    }
}