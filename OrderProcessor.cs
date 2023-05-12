using System.Globalization;
using FileParser.Entities;

namespace FileParser
{
    public class OrderProcessor
    {
        #region Fields

        private string _directoryPath = string.Empty;

        #endregion

        #region Properties

        public Order CurrentOrder { get; set; }
        public OrderHeader CurrentHeader { get; set; }
        public OrderAddress CurrentAddress { get; set; }
        public List<OrderDetail> CurrentDeets { get; set; }
        public List<OrderError> CurrentErrors { get; set; }
        public List<Order> AllOrdersProcessed { get; set; }
        public List<OrderError> AllErrorsCaptured { get; set; }

        #endregion

        #region Constructors

        public OrderProcessor(string directoryPath)
        {
            _directoryPath = directoryPath;
        }

        #endregion

        #region Methods

        public List<Order> ParseFiles(string filePath)
        {
            AllOrdersProcessed = new List<Order>();
            AllErrorsCaptured = new List<OrderError>();
            CurrentOrder = new Order();
            CurrentAddress = new OrderAddress();
            CurrentDeets = new List<OrderDetail>();
            CurrentErrors = new List<OrderError>();

            var files = Directory.GetFiles(filePath);

            // For each file in the directory...
            foreach (var file in files)
            {
                using var sr = new StreamReader(file);

                // For each line in the file...
                while (sr.ReadLine() is { } line)
                {
                    if (line.Length >= 3)
                    {
                        var lineType = int.Parse(line[..3]);

                        switch (lineType)
                        {
                            case 100:
                                // If 100 encountered and currentOrder not null, then processing new order.
                                // Commit order to list.
                                if (CurrentOrder != null)
                                {
                                    // Load current order
                                    if (CurrentOrder != null && CurrentAddress != null && CurrentDeets != null &&
                                        CurrentDeets.Count > 0)
                                    {
                                        // Load errors if any
                                        if(CurrentErrors.Count > 0)
                                            CurrentOrder.Errors.AddRange(CurrentErrors);

                                        // Load details
                                        CurrentOrder.Details.AddRange(CurrentDeets);

                                        // Add to all orders processed
                                        AllOrdersProcessed.Add(CurrentOrder);
                                    }
                                        

                                    // Clear out old contents
                                    CurrentOrder = new Order();
                                    CurrentOrder.Details = new List<OrderDetail>();
                                    CurrentDeets = new List<OrderDetail>();
                                    CurrentErrors = new List<OrderError>();
                                }
                                CurrentOrder.Header = ParseOrderHeader(line);
                                break;
                            case 200:
                                CurrentOrder.Address = ParseOrderAddress(line);
                                break;
                            case 300:
                                if(CurrentDeets == null || CurrentDeets.Count < 1)
                                    CurrentDeets = new List<OrderDetail>();

                                CurrentDeets.Add(ParseOrderDetail(line));
                                break;
                            default:
                                CurrentErrors.Add(GenerateErrorEntry(line, new InvalidOperationException("There was an unspecified error in this line.")));
                                break;
                        }
                    }

                    
                }
            }

            return AllOrdersProcessed;
        }

        public OrderHeader ParseOrderHeader(string line)
        {
            try
            {
                return new OrderHeader()
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
                };
            }
            catch (Exception ex)
            {
                AllErrorsCaptured.Add(GenerateErrorEntry(line, ex));
                return new OrderHeader();       // return empty instance to indicate error while continuing to process
            }
        }

        public OrderDetail ParseOrderDetail(string line)
        {
            try
            {
                return new OrderDetail()
                {
                    LineNumber = int.Parse(line.Substring(3, 2).Trim()),
                    Quantity = int.Parse(line.Substring(5, 5).Trim()),
                    CostEach = decimal.Parse(line.Substring(10, 10).Trim()),
                    TotalCost = decimal.Parse(line.Substring(20, 10).Trim()),
                    Description = line.Substring(30, 50).Trim()
                };
            }
            catch (Exception ex)
            {
                AllErrorsCaptured.Add(GenerateErrorEntry(line, ex));
                return new OrderDetail();       // return empty instance to indicate error while continuing to process
            }
        }

        public OrderAddress ParseOrderAddress(string line)
        {
            try
            {
                return new OrderAddress()
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
                AllErrorsCaptured.Add(GenerateErrorEntry(line, ex));
                return new OrderAddress();      // return empty instance to indicate error while continuing to process
            }
        }

        private static OrderError GenerateErrorEntry(string line, Exception ex) =>
            new()
            {
                Success = false,
                ErrorDate = DateTime.Now,
                ErrorMessage = $"Message:  {ex.Message}{Environment.NewLine}Source:  {ex.Source}{Environment.NewLine}Target Site:  {ex.TargetSite}",
                Line = line,
                Ex = ex
            };

        #endregion
    }
}
