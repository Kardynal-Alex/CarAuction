namespace Auction.WepApi.Models.Lot
{
    public class LotStateViewModel
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }
        public string FutureOwnerId { get; set; }
        public int CountBid { get; set; }
        public int LotId { get; set; }
    }
}
