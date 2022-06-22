using System;

namespace Auction.BLL.Validation
{
    /// <summary>
    /// Precognitions class
    /// </summary>
    public static class Precognitions
    {
        /// <summary>
        /// Check if string is empty or null
        /// </summary>
        /// <param name="model"></param>
        /// <param name="message"></param>
        /// <exception cref="AuctionException"></exception>
        public static void StringIsNullOrEmpty(string model, string message = null)
        {
            if (string.IsNullOrEmpty(model))
            {
                throw new AuctionException(message ?? "String is null or empty");
            }
        }
        /// <summary>
        /// Check if number is not number or negative
        /// </summary>
        /// <param name="model"></param>
        /// <param name="message"></param>
        /// <exception cref="AuctionException"></exception>
        public static void IntIsNotNumberOrNegative(int model, string message = null)
        {
            var number = IntIsNotNumber(model);  
            if (number < 0)
            {
                throw new AuctionException(message ?? "Number is less than zero");
            }
        }
        /// <summary>
        /// Validate if number is not number
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="AuctionException"></exception>
        public static int IntIsNotNumber(int model)
        {
            if (!int.TryParse(model.ToString(), out int number))
            {
                throw new AuctionException("Is not number");
            }
            return number;
        }
        /// <summary>
        /// Check if guid is not null or is correct
        /// </summary>
        /// <param name="model"></param>
        /// <param name="message"></param>
        /// <exception cref="AuctionException"></exception>
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
    }
}
