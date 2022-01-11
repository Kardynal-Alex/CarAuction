using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Auction.DAL.Entities
{
    /// <summary>
    /// Comment entity.
    /// Contains comments properties.
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Primary Key
        /// Gets and sets lotsate id.
        /// </summary>
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// Gets and sets author who create commnet.
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// Gets and sets text of who create commnet.
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Gets and sets datetime of creation commnet.
        /// </summary>
        [JsonSchemaDate]
        public DateTime DateTime { get; set; }
        /// <summary>
        /// Gets and sets lotId which belong comments.
        /// </summary>
        public int LotId { get; set; }
        /// <summary>
        /// Gets and sets userId who create comment.
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Gets and sets bool property is bid(place auto after usets bidding)
        /// </summary>
        public bool IsBid { get; set; }
    }
}
