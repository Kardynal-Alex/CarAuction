using System;
using System.Collections.Generic;

namespace PDFGenerator.Models
{
    public class LotData
    {
        public int Id { get; set; }
        public string NameLot { get; set; }
        public double StartPrice { get; set; }
        public bool IsSold { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }

        public string UserId { get; set; }
        public UserData User { get; set; }

        public DateTime StartDateTime { get; set; }
        public double CurrentPrice { get; set; }
        public int Year { get; set; }

        public ImagesData Images { get; set; }
    }
}
