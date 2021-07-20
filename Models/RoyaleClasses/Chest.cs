using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoyaleTrackerAPI.Models.RoyaleClasses
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
