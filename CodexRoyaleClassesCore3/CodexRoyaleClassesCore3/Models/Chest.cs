using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CodexRoyaleClassesCore3.Models
{
    public class Chest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Name { get; set; }

        public string Url { get; set; }

        [NotMapped]
        public int Index { get; set; }
    }
}
