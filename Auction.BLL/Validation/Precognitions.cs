using System;

namespace Auction.BLL.Validation
{
    public static class Precognitions
    {
        public static void StringIsNullOrEmpty(string model, string message = null)
        {
            if (string.IsNullOrEmpty(model))
            {
                throw new AuctionException(message ?? "String is null or empty");
            }
        }

        public static void IntIsNotNumberOrNegative(int model, string message = null)
        {
            var number = IntIsNotNumber(model);  
            if (number < 0)
            {
                throw new AuctionException(message ?? "Number is less than zero");
            }
        }
     
        public static int IntIsNotNumber(int model)
        {
            if (!int.TryParse(model.ToString(), out int number))
            {
                throw new AuctionException("Is not number");
            }
            return number;
        }
      
        public static void GuidIsNullOrEmpty(Guid model,string message = null)
        {
            if (Guid.Empty == model)  
            {
                throw new AuctionException("Guid is empty");
            }
            if (!Guid.TryParse(model.ToString(), out Guid _)) 
            {
                throw new AuctionException(message ?? "Guid is not correct");
            }
        }

        public enum CompareOperator
        {
            Less,
            Greater,
            LessEqual,
            GreaterEqual,
            Equal
        }

        public static void CompareTwoNumbers(int a, int b, CompareOperator @operator)
        {
            if (!int.TryParse(a.ToString(), out int _) || !int.TryParse(b.ToString(), out int _))
                throw new AuctionException("Number is incorrect format");

                switch (@operator)
            {
                case CompareOperator.Less:
                    {
                        if (a >= b)
                            throw new AuctionException("Numer a must be < than b");
                        break;
                    }
                case CompareOperator.Greater:
                    {
                        if (a <= b)
                            throw new AuctionException("Numer a must be > than b");
                        break;
                    }
                case CompareOperator.LessEqual:
                    {
                        if (a > b)
                            throw new AuctionException("Numer a must be <= than b");
                        break;
                    }
                case CompareOperator.GreaterEqual:
                    {
                        if (a < b)
                            throw new AuctionException("Numer a must be >= than b");
                        break;
                    }
                case CompareOperator.Equal:
                    {
                        if (a != b)
                            throw new AuctionException("Numbers must be equal");
                        break;
                    }
                default:
                    throw new AuctionException("Incorect operator");
            }
        }
    }
}
