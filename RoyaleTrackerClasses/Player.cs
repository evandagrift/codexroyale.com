using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RoyaleTrackerClasses
{
    public class Player
    {
        //this doubles as a class for Newtonsoft Json and EF Core DB Models
        [Key]
        public int Id { get; set; }
        public string Tag { get; set; }
        public int TeamId { get; set; }
        public string Name { get; set; }
        public string UpdateTime { get; set; }


        public string ClanTag { get; set; }
        public int CurrentFavouriteCardId { get; set; }
        [NotMapped]
        public Card CurrentFavouriteCard { get; set; }

        public int CurrentDeckId { get; set; }

        [NotMapped]
        public List<Card> CurrentDeck { get; set; }
        [NotMapped]
        public Deck Deck { get { return new Deck(CurrentDeck); } set { } }

        [NotMapped]
        public Clan Clan { get; set; }

        public string Role { get; set; }
        public string LastSeen { get; set; }


        public int ExpLevel { get; set; }
        public int Trophies { get; set; }
        public int BestTrophies { get; set; }
        public int StarPoints { get; set; }

        public int Wins { get; set; }
        public int Losses { get; set; }

        public int Donations { get; set; }
        public int DonationsReceived { get; set; }
        public int TotalDonations { get; set; }
        public int CardsDiscovered { get; set; }
        [NotMapped]
        public List<Card> Cards { get; set; }
        [NotMapped]
        public int CardsInGame { get; set; }
        public int ClanCardsCollected { get; set; }

        public void SetDeck()
        {
            Deck.Card1Id = CurrentDeck[0].Id;
            Deck.Card2Id = CurrentDeck[1].Id;
            Deck.Card3Id = CurrentDeck[2].Id;
            Deck.Card4Id = CurrentDeck[3].Id;
            Deck.Card5Id = CurrentDeck[4].Id;
            Deck.Card6Id = CurrentDeck[5].Id;
            Deck.Card7Id = CurrentDeck[6].Id;
            Deck.Card8Id = CurrentDeck[7].Id;
        }
    }
}

/*

Keeping this here for now


//the html that displays the clan card used in the pop down menu's and header of searched clan
public override string ToString()
{
    if (Clan != null)
    {
        string returnString = "<div class=\"card card-group\"> <div class=\"container-fluid d-inline-flex\"> <div class=\"col-2\"> <p><b>Name:</b>" + Name + "</p> <p><b>Tag:</b>" + Tag + "</p> <p><b>Level:</b>" + ExpLevel.ToString() + "</p>";

        if (ExpLevel == 13) { returnString += "<p><b>Star Points:</b>" + StarPoints.ToString() + "</p>"; }

        returnString += "<p><b>Current Tropies:</b>" + Trophies.ToString() + "</p><p><b>Highest Trophies:</b>" + BestTrophies + "</p></div>";

        returnString += "<div class=\"col-2\"><p><b>All Time Wins:</b>" + Wins.ToString() + "</p><p><b>All Time Losses:</b>" + Losses.ToString() + "</p>";
        Console.WriteLine();
        returnString += "<p><b>Current Favorite Card:</b>" + CurrentFavouriteCard.Name + "</p><img class=\"text-center\" src=\"" + CurrentFavouriteCard.Url + "\" width=\"64px\" />";
        returnString += "<p><b>Cards Discovered:</b>" + CardsDiscovered.ToString() + "/" + CardsInGame.ToString() + "</p></div>";

        returnString += "<div class=\"col-3\">";
        if (Clan.Name != null)
        {
            returnString += "<p><b>Clan Name:</b>" + Clan.Name + "</p>";
        }
        if (Clan.Tag != null)
        {
            returnString += "<p><b>Clan Tag:</b>" + ClanTag + "</p>";
        }
        else { returnString += "<h2 class=\"text-center m-2\">Not In a Clan</h2>"; }

        returnString += "<p><b>Recent Donations:</b>" + Donations.ToString() + "</p><p><b>Recent Donations Recieved:</b>" + DonationsReceived.ToString() + "</p>";
        returnString += "<p><b>Total Donations:</b>" + TotalDonations.ToString() + "</p><p><b>Total Donations Recieved:</b>" + ClanCardsCollected.ToString() + "</p></div>";


        returnString += "<div class=\"col-5 m-0\"><div class=\"text-center\"><p><b>Current Deck</b></p> " + Deck.ToString() + "<p>Profile Updated:" + UpdateTime + "</p></div></div></div></div>";

        return returnString;
    }
    else { return null; }
}
*/