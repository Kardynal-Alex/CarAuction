namespace Auction.BLL.DTO.Lot
{
    public class LotStateDTO
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }
        public string FutureOwnerId { get; set; }
        public int CountBid { get; set; }
        public int LotId { get; set; }
    }
}