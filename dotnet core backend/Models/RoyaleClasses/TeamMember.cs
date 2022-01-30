using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RoyaleTrackerClasses
{
    [NotMapped]
    public class TeamMember
    {
        public string Tag { get; set; } //player tag fetched from json
        public string Name { get; set; } //player name fetched from json
        public int TrophyChange { get; set; }//how many trophies player gained or lost as result of game

        public int StartingTrophies { get; set; } //player's starting trophies fetched from json
        public int Crowns { get; set; } //crowns won in game, 1 for each princess tower destroyed, total of 3 if king tower is destroyed
        public int KingTowerHitPoints { get; set; } //main tower remaining HP

        //remaining hp on the princess towers returns null if at full hp or if dead
        //if either returns -1, use opponents crowns to find out if tower alive or dead
        public int PrincessTowerA 
        { 
            get 
            { 
                return !(PrincessTowersHitPoints == null) ? PrincessTowersHitPoints[0] : -1; 
            } set { }
        }

        //remaining hp on the princess towers returns null if at full hp or if dead
        //if either returns -1, use opponents crowns to find out if tower alive or dead
        public int PrincessTowerB 
        { 
            get 
            { 
                if (PrincessTowersHitPoints != null && PrincessTowersHitPoints.Count > 1) 
                { return PrincessTowersHitPoints[1]; } 
                else return -1; 
            } 
            set { } 
        }

        // variable to pull the princess tower hit points from json if available
        [NotMapped]
        public List<int> PrincessTowersHitPoints { get; set; }
        //clan class consumes the json
        [NotMapped]
        public Clan Clan { get; set; }


        //their deck consumed from json by list of Card classes
        [NotMapped]
        public List<Card> Cards { get; set; }
    }
}
