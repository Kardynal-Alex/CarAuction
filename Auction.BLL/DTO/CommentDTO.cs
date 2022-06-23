using NJsonSchema.Annotations;
using System;

namespace Auction.BLL.DTO
{
    public class CommentDTO
    {
        public Guid Id { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        [JsonSchemaDate]
        public DateTime DateTime { get; set; }
        public int LotId { get; set; }
        public string UserId { get; set; }
        public bool IsBid { get; set; }
    }
}
