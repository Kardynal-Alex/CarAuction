using NJsonSchema.Annotations;
using System;
using TypeGen.Core.TypeAnnotations;

namespace Auction.WepApi.Models
{
    [ExportTsClass]
    public class CommentViewModel
    {
        public Guid Id { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        [JsonSchemaDate]
        public DateTime DateTime { get; set; }
        public string LotId { get; set; }
        public string UserId { get; set; }
        public bool IsBid { get; set; }
    }
}
