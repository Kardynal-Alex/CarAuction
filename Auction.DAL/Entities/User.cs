using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Auction.DAL.Entities
{
    /// <summary>
    /// User entity (is identity).
    /// Contains user properties.
    /// </summary>
    public class User : IdentityUser
    {
        /// <summary>
        /// Gets and sets user name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets and sets user surname.
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// Gets and sets user role.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Gets and sets user lots.
        /// ICollection of <see cref="Lots"/> reference
        /// </summary>
        public ICollection<Lot> Lots { get; set; }
    }
}
