using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RoyaleTrackerClasses
{
    public class TrackedPlayer
    {
        public int Id { get; set; }
        public string Tag { get;set; }

        //priorities are "normal" and "high"
        public string Priority { get; set; }
    }
}
