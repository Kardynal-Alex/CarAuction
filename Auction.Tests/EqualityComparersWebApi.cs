using Auction.WepApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Auction.Tests
{
    public class LotViewModelEqualityComparer : IEqualityComparer<LotViewModel>
    {
        public bool Equals([AllowNull] LotViewModel x, [AllowNull] LotViewModel y)
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

        public int GetHashCode([DisallowNull] LotViewModel obj)
        {
            return obj.GetHashCode();
        }
    }

    public class LotStateViewModelEqualityComparer : IEqualityComparer<LotStateViewModel>
    {
        public bool Equals([AllowNull] LotStateViewModel x, [AllowNull] LotStateViewModel y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.OwnerId == y.OwnerId && x.FutureOwnerId == y.FutureOwnerId
                && x.CountBid == y.CountBid && x.LotId == y.LotId;
        }

        public int GetHashCode([DisallowNull] LotStateViewModel obj)
        {
            return obj.GetHashCode();
        }
    }

    public class FavoriteViewModelEqualityComparer : IEqualityComparer<FavoriteViewModel>
    {
        public bool Equals([AllowNull] FavoriteViewModel x, [AllowNull] FavoriteViewModel y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.UserId == y.UserId && x.LotId == y.LotId;
        }

        public int GetHashCode([DisallowNull] FavoriteViewModel obj)
        {
            return obj.GetHashCode();
        }
    }

    public class CommentViewModelEqualityComparer : IEqualityComparer<CommentViewModel>
    {
        public bool Equals([AllowNull] CommentViewModel x, [AllowNull] CommentViewModel y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.Author == y.Author && x.Text == y.Text && x.DateTime == y.DateTime
                && x.LotId == y.LotId && x.UserId == y.UserId && x.IsBid == y.IsBid;
        }

        public int GetHashCode([DisallowNull] CommentViewModel obj)
        {
            return obj.GetHashCode();
        }
    }

    public class UserViewModelEqualityComparer : IEqualityComparer<UserViewModel>
    {
        public bool Equals([AllowNull] UserViewModel x, [AllowNull] UserViewModel y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.Name == y.Name && x.Surname == y.Surname
                && x.Role == y.Role && x.Email == y.Email;
        }

        public int GetHashCode([DisallowNull] UserViewModel obj)
        {
            return obj.GetHashCode();
        }
    }

    public class ImagesViewModelEqualityComparer : IEqualityComparer<ImagesViewModel>
    {
        public bool Equals([AllowNull] ImagesViewModel x, [AllowNull] ImagesViewModel y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.Image1 == y.Image1 && x.Image2 == y.Image2 && x.Image3 == y.Image3
                && x.Image4 == y.Image4 && x.Image5 == y.Image5 && x.Image6 == y.Image6
                && x.Image7 == y.Image7 && x.Image8 == y.Image8 && x.Image9 == y.Image9;
        }

        public int GetHashCode([DisallowNull] ImagesViewModel obj)
        {
            return obj.GetHashCode();
        }
    }
}
