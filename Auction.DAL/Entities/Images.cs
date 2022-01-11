using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auction.DAL.Entities
{
    /// <summary>
    /// Images entity.
    /// Contains lot properties.
    /// </summary>
    public class Images
    {
        /// <summary>
        /// Primary Key
        /// Gets and sets Images id.
        /// </summary>
        [Key]
        [ForeignKey("Lot")]
        public int Id { get; set; }
        /// <summary>
        /// Gets and sets Images Image1
        /// </summary>
        public string Image1 { get; set; }
        /// <summary>
        /// Gets and sets Images Image2
        /// </summary>
        public string Image2 { get; set; }
        /// <summary>
        /// Gets and sets Images Image3
        /// </summary>
        public string Image3 { get; set; }
        /// <summary>
        /// Gets and sets Images Image4
        /// </summary>
        public string Image4 { get; set; }
        /// <summary>
        /// Gets and sets Images Image5
        /// </summary>
        public string Image5 { get; set; }
        /// <summary>
        /// Gets and sets Images Image6
        /// </summary>
        public string Image6 { get; set; }
        /// <summary>
        /// Gets and sets Images Image7
        /// </summary>
        public string Image7 { get; set; }
        /// <summary>
        /// Gets and sets Images Image8
        /// </summary>
        public string Image8 { get; set; }
        /// <summary>
        /// Gets and sets Images Image9
        /// </summary>
        public string Image9 { get; set; }
        /// <summary>
        /// Gets and sets lot 
        /// </summary>
        public Lot Lot { get; set; }
    }
}
