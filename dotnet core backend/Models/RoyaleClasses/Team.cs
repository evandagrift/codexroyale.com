using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RoyaleTrackerClasses
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public bool TwoVTwo {get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public string Name2 { get; set; }
        public string Tag2 { get; set; }
    }
}
