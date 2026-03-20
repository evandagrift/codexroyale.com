using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClashFeeder.Models
{
    public class Card
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Rarity { get; set; }
        public string ElixirCost { get; set; }
        public string Url { get; set; }
        public string UrlEvolved { get; set; }
        public bool? EventCard { get; set; }

        [NotMapped]
        public IDictionary<string, string> IconUrls { get; set; }

        public void SetUrls()
        {
            if (IconUrls != null)
            {
                if (!String.IsNullOrEmpty(IconUrls["medium"]))
                {
                    Url = IconUrls["medium"];
                }
                if (!String.IsNullOrEmpty(IconUrls["evolutionMedium"]))
                {
                    UrlEvolved = IconUrls["evolutionMedium"];
                }
            }
        }
    }
}
