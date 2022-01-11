using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auction.BLL.DTO
{
    /// <summary>
    /// <see cref="CommentDTO"/> data transfer class
    /// </summary>
    public class CommentDTO
    {
        /// <summary>
        /// Primary Key
        // CommentDTO id.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// CommentDTO Author
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// CommentDTO text vomment
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// CommentDTO date of creation comment
        /// </summary>
        [JsonSchemaDate]
        public DateTime DateTime { get; set; }
        /// <summary>
        /// CommentDTO lotId
        /// </summary>
        public int LotId { get; set; }
        /// <summary>
        /// ComentDTO userId
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// CommentDTO IsBid
        /// </summary>
        public bool IsBid { get; set; }
    }
}
