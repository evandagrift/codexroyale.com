import React, { Component } from "react";

import Card from "../components/Card";
import Deck from "../components/Deck";
import Time from "./Time";
import { Redirect } from "react-router-dom";
import { getPlayerDataAsync } from "../Utilities/axios-functions";

class Player extends Component {
  constructor(props) {
    super(props);
    this.state = {
      player: []
    };
  }

  async componentDidMount() {
    const { playerTag } = this.props;

    //call player at given Tag

    try {
      let responsePlayer = await getPlayerDataAsync(playerTag);

      if(responsePlayer)
      {

        this.setState({ player: responsePlayer });

      }
      else this.setState({redirect:true});


    } catch {this.setState({redirect:true})}
  } 

  render() {
    let draw = "";
    if(this.state.redirect){
      return <Redirect to="/" />;
    }
     if(this.state.player && this.state.player.Name)
    {
      draw = (
  <div className="card">
    <div className="card-body text-center">
      <div className="row d-inline-flex text-center">
        <div className="col">
          <div className="row d-inline-flex">
            <h1 className="d-inline">{this.state.player.Name}</h1>
            <h6 className="d-inline">(Lvl.{this.state.player.ExpLevel})</h6>
            <p className="d-inline">
              <i>Tag:{this.state.player.Tag}</i>
            </p>
          </div>
          <p className="text-center m-0">
            <b>Current Deck</b>
          </p>
          <Deck deck={this.state.player.Deck} />

          <p>
            <b>Last Seen:</b>
            {(this.state.player.Clan!=null)?<Time time={this.state.player.LastSeen} />:"Unkown"}
            
          </p>
        </div>

        <div className="col">
          <div className="text-center">
            <p>
              <b>Trophies:</b>
              {this.state.player.Trophies}
            </p>

            <p>
              <b>Best Trophies:</b>
              {this.state.player.BestTrophies}
            </p>
            <p>
              <b>Wins:</b>
              {this.state.player.Wins}
            </p>
            <p>
              <b>Losses:</b>
              {this.state.player.Losses}
            </p>
            <p>
              <b>Three Crown Wins:</b>
              {this.state.player.ThreeCrownWins}
            </p>

            <p>
              <b>Star Points:</b>
              {this.state.player.StarPoints}
            </p>

            <p>
              <b>Cards Discovered:</b>
              {this.state.player.CardsDiscovered}
            </p>

            <p>
              <b>Total Games:</b>
              {this.state.player.BattleCount}
            </p>
          </div>
        </div>

        <div className="col">
          <p>
            <b>Clan Name:</b>
            {(this.state.player.Clan!=null)? this.state.player.Clan.Name:"Not In A Clan"}
          </p>

          <p>
            <b>Clan Tag:</b>
            {(this.state.player.Clan!=null)? this.state.player.ClanTag:"Not In A Clan"}
          </p>
          <p>
            <b>Role:</b>
            {(this.state.player.Clan!=null)?this.state.player.Role:"Not In A Clan"}
          </p>
          <p>
            <b>Recent Donations:</b>
            {(this.state.player.Clan!=null)?this.state.player.Donations:"Not In A Clan"}
          </p>
          <p>
            <b>Recent Donations Recieved:</b>
            {(this.state.player.Clan!=null)?this.state.player.DonationsReceived:"Not In A Clan"}
          </p>
          <p>
            <b>Total Donations:</b>
            {(this.state.player.Clan!=null)?this.state.player.TotalDonations:"Not In A Clan"}
          </p>
          <p>
            <b>Total Donations Recieved:</b>
            {(this.state.player.Clan!=null)?this.state.player.ClanCardsCollected:"Not In A Clan"}
          </p>

          <p>
            <b>War Day Wins:</b>
            {(this.state.player.Clan!=null)?this.state.player.WarDayWins:"Not In A Clan"}
          </p>
        </div>
      </div>
    </div>
  </div>
);


    }
    else draw = (<h1>Loading Player Data</h1>)

    return <div>{draw}</div>;
  }
}
export default Player;

    /*
    if (tag != '') {
      try {
        const responsePlayer = await axios.post("players/official/" + FormatTag(tag));
        setPlayer(responsePlayer.data);

        // let response = [];
        
        //   while(response.length == 0)
        //   {
        //     //times out to give the data feeder to catch up with the registered player
        //     await new Promise(r => setTimeout(r, 2000));
        //     const responseBattles = await axios.get("battles/player/" + FormatTag(tag));

        //     if(responseBattles.data != null) response = responseBattles.data;

        //     setBattles(response);
        //   }

      } catch {}
    }
*/