
namespace Auction.DAL.Entities
{
    public class EmailMessage
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public byte[] PDFFile { get; set; }
    }
}
