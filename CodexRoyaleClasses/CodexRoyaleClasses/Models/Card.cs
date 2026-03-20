using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CodexRoyaleClasses.Models
{
    public class Card
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int? ElixirCost { get; set; }
        public bool? EventCard { get; set; }

        [NotMapped]
        public IDictionary<string, string> IconUrls { get; set; }

        public void SetUrl()
        {
            if (IconUrls != null)
            {
                Url = IconUrls["medium"];
            }
        }
    }
}
