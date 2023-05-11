using System.Globalization;
using FileParser.Entities;

namespace FileParser // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var directoryPath = PrintTitleScreen();
            try
            {
                ParseFiles(Path.GetFullPath(directoryPath) ?? throw new InvalidOperationException("Please provide a valid path."));
            }
            catch (Exception)
            {
                Console.Clear();
                directoryPath = PrintTitleScreen();
            }
        }

        public static string? PrintTitleScreen()
        {
            Console.WriteLine("WELCOME TO THE FILE PARSER");
            Console.Write("Please enter the directory for the order files:  ");
            return Console.ReadLine();
        }

        public static void ParseFiles(string filePath)
        {
            var files = Directory.GetFiles(filePath);

            foreach (var file in files)
            {
                using var sr = new StreamReader(file);
                while (sr.ReadLine() is { } line)
                {
                    if (line.Length >= 3)
                    {
                        var lineType = int.Parse(line[..3]);
                        switch (lineType)
                        {
                            case 100:
                                ParseOrder(line);
                                break;
                            case 200:
                                ParseAddress(line);
                                break;
                            case 300:
                                ParseOrderDetail(line);
                                break;
                            default:
                                GenerateErrorEntry(line, new InvalidOperationException("There was an unspecified error in this line."));
                                break;
                        }
                    }
                }
            }

            
        }
    }
}