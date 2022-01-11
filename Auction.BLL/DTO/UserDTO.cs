
namespace Auction.BLL.DTO
{
    /// <summary>
    /// <see cref="UserDTO"/> data transfer class
    /// </summary>
    public class UserDTO
    {
        /// <summary>
        /// UserDTO Id(primary key)
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// UserDTO Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// UserDTO SurName
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// UserDTO Role
        /// </summary>
        public string Role { get; set; }
        /// <summary>
        /// UserDTO Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// UserDTO Password
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Needed for confirm email
        /// </summary>
        public string ClientURI { get; set; }
    }
}
