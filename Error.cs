namespace FileParser
{
    public class Error
    {
        public bool Success { get; set; }
        public DateTime ErrorDate { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Line { get; set; }
    }
}
