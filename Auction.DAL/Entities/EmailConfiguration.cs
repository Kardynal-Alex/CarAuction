
namespace Auction.DAL.Entities
{
    /// <summary>
    /// Email Configuration
    /// </summary>
    public class EmailConfiguration
    {
        /// <summary>
        /// Email from
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// SmtpServer
        /// </summary>
        public string SmtpServer { get; set; }
        /// <summary>
        /// Port
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// Email from
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Password for connecting to email
        /// </summary>
        public string Password { get; set; }
    }
}
