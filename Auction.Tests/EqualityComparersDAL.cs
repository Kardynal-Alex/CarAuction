using Auction.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Auction.Tests
{
    public class LotEqualityComparer : IEqualityComparer<Lot>
    {
        public bool Equals([AllowNull] Lot x, [AllowNull] Lot y)
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

        public int GetHashCode([DisallowNull] Lot obj)
        {
            return obj.GetHashCode();
        }
    }

    public class LotStateEqualityComparer : IEqualityComparer<LotState>
    {
        public bool Equals([AllowNull] LotState x, [AllowNull] LotState y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.OwnerId == y.OwnerId && x.FutureOwnerId == y.FutureOwnerId
                && x.CountBid == y.CountBid && x.LotId == y.LotId;
        }

        public int GetHashCode([DisallowNull] LotState obj)
        {
            return obj.GetHashCode();
        }
    }

    public class CommentEqualityComparer : IEqualityComparer<Comment>
    {
        public bool Equals([AllowNull] Comment x, [AllowNull] Comment y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.Author == y.Author && x.Text == y.Text && x.DateTime == y.DateTime
                && x.LotId == y.LotId && x.UserId == y.UserId && x.IsBid == y.IsBid;
        }

        public int GetHashCode([DisallowNull] Comment obj)
        {
            return obj.GetHashCode();
        }
    }

    public class FavoriteEqualityComparer : IEqualityComparer<Favorite>
    {
        public bool Equals([AllowNull] Favorite x, [AllowNull] Favorite y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.UserId == y.UserId && x.LotId == y.LotId;
        }

        public int GetHashCode([DisallowNull] Favorite obj)
        {
            return obj.GetHashCode();
        }
    }

    public class UserEqualityComparer : IEqualityComparer<User>
    {
        public bool Equals([AllowNull] User x, [AllowNull] User y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.Name == y.Name && x.Surname == y.Surname
                && x.Role == y.Role && x.Email == y.Email;
        }

        public int GetHashCode([DisallowNull] User obj)
        {
            return obj.GetHashCode();
        }
    }

    public class ImagesEqualityComparer : IEqualityComparer<Images>
    {
        public bool Equals([AllowNull] Images x, [AllowNull] Images y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.Image1 == y.Image1 && x.Image2 == y.Image2 && x.Image3 == y.Image3
                && x.Image4 == y.Image4 && x.Image5 == y.Image5 && x.Image6 == y.Image6
                && x.Image7 == y.Image7 && x.Image8 == y.Image8 && x.Image9 == y.Image9;
        }

        public int GetHashCode([DisallowNull] Images obj)
        {
            return obj.GetHashCode();
        }
    }
}
