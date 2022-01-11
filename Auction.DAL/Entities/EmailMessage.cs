
namespace Auction.DAL.Entities
{
    /// <summary>
    /// Contains emailMessage properties.
    /// </summary>
    public class EmailMessage
    {
        /// <summary>
        /// Send email user
        /// </summary>
        public string To { get; set; }
        /// <summary>
        /// Header Text of the letter
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// Some contexnt(alos JWT token)
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// For sending PDF file
        /// </summary>
        public byte[] PDFFile { get; set; }
    }
}
