using System;

namespace Auction.WepApi.Models
{
    /// <summary>
    /// User View Model
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// UserViewModel Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// UserViewModel Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// UserViewModel Surname
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// UserViewModel Role
        /// </summary>
        public string Role { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// UserViewModel Password
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Needed for confirm email
        /// </summary>
        public string ClientURI { get; set; }
    }
}
