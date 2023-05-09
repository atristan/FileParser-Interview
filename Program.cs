using System.Globalization;
using FileParser;

namespace FileParser // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        #region Properies

        public static List<Order> Orders = new List<Order>();
        public static Order CurrentOrder { get; set; }
        public static Address CurrentAddress { get; set; }
        public static List<OrderDetail> CurrentOrderDetails { get; set; }
        public static List<Error> Errors => new List<Error>();

        #endregion

        private static void Main(string[] args)
        {
            Console.WriteLine("Hello.  Please provide the path of the file you'd like to parse:");

            try
            {
                ParseFile(Console.ReadLine() ?? throw new InvalidOperationException("You must provide a valid path."));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
                ParseFile(Console.ReadLine());
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

        private static void ParseOrder(string line)
        {
            try
            {
                CurrentOrder = new Order()
                {
                    OrderNumber = int.Parse(line.Substring(3, 10).Trim()),
                    TotalItems = int.Parse(line.Substring(13, 5).Trim()),
                    TotalCost = decimal.Parse(line.Substring(18, 10).Trim()),
                    OrderDate = DateTime.ParseExact(line.Substring(28, 19).Trim(), "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                    CustomerName = line.Substring(47, 50).Trim(),
                    CustomerPhone = line.Substring(97, 30).Trim(),
                    CustomerEmail = line.Substring(127, 50).Trim(),
                    Paid = line[177] == '1',
                    Shipped = line[178] == '1',
                    Completed = line[179] == '1',
                    Success = true,
                    Processed = true,
                    ErrorMessage = string.Empty

                }; 
                
                //Console.WriteLine(orderHeader.ToString());
            }
            catch (Exception ex)
            {
                GenerateErrorEntry(line, ex);
            }

            Orders.Add(CurrentOrder);
        }

        private static void ParseAddress(string line)
        {
            try
            {
                CurrentAddress = new Address()
                {
                    AddressLine1 = line.Substring(3, 50).Trim(),
                    AddressLine2 = line.Substring(53, 50).Trim(),
                    City = line.Substring(103, 50).Trim(),
                    State = line.Substring(153, 2).Trim(),
                    Zip = line.Substring(155, 10).Trim(),
                };
            }
            catch (Exception ex)
            {
                GenerateErrorEntry(line, ex);
            }

        }

        private static void ParseOrderDetail(string line)
        {
            CurrentOrderDetails ??= new List<OrderDetail>();

            try
            {
                CurrentOrderDetails.Add(new OrderDetail()
                {
                    LineNumber = int.Parse(line.Substring(3, 2).Trim()),
                    Quantity = int.Parse(line.Substring(5, 5).Trim()),
                    CostEach = decimal.Parse(line.Substring(10, 10).Trim()),
                    TotalCost = decimal.Parse(line.Substring(20, 10).Trim()),
                    Description = line.Substring(30, 50).Trim()
                });
            }
            catch (Exception ex)
            {
                GenerateErrorEntry(line, ex);
            }
        }

        private static void GenerateErrorEntry(string line, Exception ex)
        {
            Errors.Add(new Error()
            {
                Success = false,
                ErrorDate = DateTime.Now,
                ErrorMessage = $"Message:  {ex.Message}{Environment.NewLine}Source:  {ex.Source}{Environment.NewLine}Target Site:  {ex.TargetSite}",
                Line = line

            });
        }
    }
}