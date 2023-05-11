using System.Globalization;
using FileParser.Entities;

namespace FileParser // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello.  Please provide the path of the file you'd like to parse:");

            try
            {
                Order.ParseFile(Console.ReadLine() ?? throw new InvalidOperationException("You must provide a valid path."));
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
                Order.ParseFile(Console.ReadLine());
            }

            foreach (var order in Orders)
            {
                Console.WriteLine(
                        $"Order:  {order.CustomerName}, Total:  {order.TotalCost}, Total Items:  {order.TotalItems}"
                    );
            }
        }

        public static void ParseFile(string filePath)
        {
            using var sr = new StreamReader(filePath);
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