using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace RoyaleTrackerClasses
{
    public class Battle
    {
        [Key]
        public int BattleId { get; set; }

        public string BattleTime { get; set; }

        //if 2v2 name will be name + " " + name2
        public string Team1Name { get; set; }

        public int Team1Id { get; set; }
        public bool Team1Win { get; set; }

        //2v2 doesn't effect trophies
        public int Team1StartingTrophies { get; set; }
        public int Team1TrophyChange { get; set; }

        //FK to the Deck Id
        public int Team1DeckAId { get; set; }
        public int Team1DeckBId { get; set; }
        public int Team1Crowns { get; set; }

        //-1 will flag full health, 0 is dead
        public int Team1KingTowerHp { get; set; }
        public int Team1PrincessTowerHpA { get; set; }
        public int Team1PrincessTowerHpB { get; set; }


        //if 2v2 name will be name + " " + name2
        public string Team2Name { get; set; }
        public int Team2Id { get; set; }
        public bool Team2Win { get; set; }

        //2v2 doesn't effect trophies
        public int Team2StartingTrophies { get; set; }
        public int Team2TrophyChange { get; set; }

        //FK to the Deck Id
        public int Team2DeckAId { get; set; }

        public int Team2DeckBId { get; set; }


        public int Team2Crowns { get; set; }

        //-1 will flag full health, 0 is dead
        public int Team2KingTowerHp { get; set; }
        public int Team2PrincessTowerHpA { get; set; }
        public int Team2PrincessTowerHpB { get; set; }

        public string Type { get; set; }
        public string DeckSelection { get; set; }
        public bool IsLadderTournament { get; set; }

        public int GameModeId { get; set; }

        [NotMapped]
        public GameMode GameMode { get; set; }


        [NotMapped]
        public List<TeamMember> Team { get; set; }

        [NotMapped]
        public List<TeamMember> Opponent { get; set; }
    }
}
