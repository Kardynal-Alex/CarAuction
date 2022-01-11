using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;

namespace Auction.WepApi.Models
{
    /// <summary>
    /// Comment View Model
    /// </summary>
    public class CommentViewModel
    {
        /// <summary>
        /// Primary Key
        // CommentViewModel id.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// CommentViewModel Author
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// CommentViewModel text comment
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// CommentViewModel date of creation comment
        /// </summary>
        [JsonSchemaDate]
        public DateTime DateTime { get; set; }
        /// <summary>
        /// CommentViewModel lotId
        /// </summary>
        public string LotId { get; set; }
        /// <summary>
        /// CommentViewModel UserId
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// CommentViewModel IsBid
        /// </summary>
        public bool IsBid { get; set; }
    }
}
