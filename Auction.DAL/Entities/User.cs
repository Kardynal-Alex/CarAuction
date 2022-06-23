using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Auction.DAL.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }
        public ICollection<Lot> Lots { get; set; }
    }
}
