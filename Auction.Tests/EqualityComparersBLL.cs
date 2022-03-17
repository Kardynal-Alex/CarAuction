using Auction.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Auction.Tests
{
    public class LotDTOEqualityComparer : IEqualityComparer<LotDTO>
    {
        public bool Equals([AllowNull] LotDTO x, [AllowNull] LotDTO y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.NameLot == y.NameLot && x.StartPrice == y.StartPrice
                && x.IsSold == y.IsSold && x.Description == y.Description && x.UserId == y.UserId
                && x.StartDateTime == y.StartDateTime && x.CurrentPrice == y.CurrentPrice
                && x.Year == y.Year;
        }

        public int GetHashCode([DisallowNull] LotDTO obj)
        {
            return obj.GetHashCode();
        }
    }

    public class LotStateDTOEqualityComparer : IEqualityComparer<LotStateDTO>
    {
        public bool Equals([AllowNull] LotStateDTO x, [AllowNull] LotStateDTO y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.OwnerId == y.OwnerId && x.FutureOwnerId == y.FutureOwnerId
                && x.CountBid == y.CountBid && x.LotId == y.LotId;
        }

        public int GetHashCode([DisallowNull] LotStateDTO obj)
        {
            return obj.GetHashCode();
        }
    }

    public class CommentDTOEqualityComparer : IEqualityComparer<CommentDTO>
    {
        public bool Equals([AllowNull] CommentDTO x, [AllowNull] CommentDTO y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.Author == y.Author && x.Text == y.Text && x.DateTime == y.DateTime
                && x.LotId == y.LotId && x.UserId == y.UserId && x.IsBid == y.IsBid;
        }

        public int GetHashCode([DisallowNull] CommentDTO obj)
        {
            return obj.GetHashCode();
        }
    }

    public class FavoriteDTOEqualityComparer : IEqualityComparer<FavoriteDTO>
    {
        public bool Equals([AllowNull] FavoriteDTO x, [AllowNull] FavoriteDTO y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.UserId == y.UserId && x.LotId == y.LotId;
        }

        public int GetHashCode([DisallowNull] FavoriteDTO obj)
        {
            return obj.GetHashCode();
        }
    }

    public class UserDTOEqualityComparer : IEqualityComparer<UserDTO>
    {
        public bool Equals([AllowNull] UserDTO x, [AllowNull] UserDTO y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.Name == y.Name && x.Surname == y.Surname
                && x.Role == y.Role && x.Email == y.Email;
        }

        public int GetHashCode([DisallowNull] UserDTO obj)
        {
            return obj.GetHashCode();
        }
    }

    public class ImagesDTOEqualityComparer : IEqualityComparer<ImagesDTO>
    {
        public bool Equals([AllowNull] ImagesDTO x, [AllowNull] ImagesDTO y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.Image1 == y.Image1 && x.Image2 == y.Image2 && x.Image3 == y.Image3
                && x.Image4 == y.Image4 && x.Image5 == y.Image5 && x.Image6 == y.Image6
                && x.Image7 == y.Image7 && x.Image8 == y.Image8 && x.Image9 == y.Image9;
        }

        public int GetHashCode([DisallowNull] ImagesDTO obj)
        {
            return obj.GetHashCode();
        }
    }

    public class AuthorDescriptionDTOEqualityComparer : IEqualityComparer<AuthorDescriptionDTO>
    {
        public bool Equals([AllowNull] AuthorDescriptionDTO x, [AllowNull] AuthorDescriptionDTO y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.LotId == y.LotId && x.Description == y.Description && x.UserId == y.UserId;
        }

        public int GetHashCode([DisallowNull] AuthorDescriptionDTO obj)
        {
            return obj.GetHashCode();
        }
    }
}
