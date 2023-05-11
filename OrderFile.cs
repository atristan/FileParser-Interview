using System.Globalization;
using FileParser.Entities;

namespace FileParser
{
    public class OrderFile
    {
        #region Properties

        public int Idx { get; set; }
        public OrderHeader Header { get; set; }
        public OrderAddress Address { get; set; }
        public List<OrderDetail> Details { get; set; }
        public OrderError Error { get; set; }

        #endregion

        #region Methods

        public void ParseOrderHeader(string line)
        {
            try
            {
                Header = new OrderHeader()
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
                GenerateErrorEntry(line, ex);
            }
        }

        public void ParseOrderDetail(string line)
        {
            Details ??= new List<OrderDetail>();

            try
            {
                Details.Add(new OrderDetail()
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

        public void ParseOrderAddress(string line)
        {
            try
            {
                Address = new OrderAddress()
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

        public void GenerateErrorEntry(string line, Exception ex)
        {
            Error = new OrderError()
            {
                Success = false,
                ErrorDate = DateTime.Now,
                ErrorMessage = $"Message:  {ex.Message}{Environment.NewLine}Source:  {ex.Source}{Environment.NewLine}Target Site:  {ex.TargetSite}",
                Line = line,
                Ex = ex
            };
        }

        #endregion
    }
}
