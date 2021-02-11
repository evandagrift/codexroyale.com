using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RoyaleTrackerClasses
{
    public class Clan
    {
        [Key]
        public int Id { get; set; }
        public string Tag { get; set; }
        public string UpdateTime { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public int BadgeId { get; set; }
        public string LocationCode { get; set; }
        [NotMapped]
        public IDictionary<string, string> Location { get; set; }
        public int RequiredTrophies { get; set; }
        public int DonationsPerWeek { get; set; }
        public string ClanChestStatus { get; set; }
        public int ClanChestLevel { get; set; }

        public int ClanScore { get; set; }

        public int ClanWarTrophies { get; set; }

        public int Members { get; set; }

        [NotMapped]
        public List<Player> MemberList { get; set; }
    }
}

/*
public override string ToString()
{
    return "<div class=\"card\"><div class=\"container-fluid d-inline-flex\"><div class=\"col-4\"><p><b>Clan Name:</b>" + Name + "</p><p><b>Clan Tag:</b>" + Tag + "</p>" +
        "<p><b>Members:</b>" + Members.ToString() + "/50</p><p><b>Type:</b>"+Type+"</p><p><b>Required Tropies:</b>" + RequiredTrophies.ToString() + "</p><p><b>Location:</b>LocationCode</p></div>" +
        "<div class=\"col-8\"><p><b>Clan Score:</b>" + ClanScore.ToString() + "</p><p><b>Clan War Trophies:</b> " + ClanWarTrophies.ToString() +
         "</p><p><b>Donations Per Week:</b>" + DonationsPerWeek.ToString() + "</p><p><b>Clan Score:</b>" + ClanScore.ToString() + "</p><p><b>Description:</b>" + Description + "</p></div></div></div>";
    }
*/