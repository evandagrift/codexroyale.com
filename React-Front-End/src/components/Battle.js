import React, { Component, memo } from "react";
import Deck from "./Deck";
import Time from "./Time";
import styles from "../cssModules/Battle.module.css";
import { getTeamDataAsync } from "../Utilities/axios-functions";
import { NavLink } from "react-router-dom";

class Battle extends Component {
  constructor(props) {
    super(props);
    this.state = {
      redirect: "",
    };
  }


  render() {
    const { battle } = this.props;
    console.log("Rendering Battle")

    // var clickPlayer1 = async () => {
    //   var playerTag = await GetPlayerTagAsync(battle.Team1Id);
    //   console.log(playerTag);
    //   this.setState({ redirect: playerTag })
    // }

    // var clickPlayer2 = async () => {
    //   var playerTag = await GetPlayerTagAsync(battle.Team2Id);
    //   this.setState({ redirect: playerTag })
    // }


    // if (this.state.redirect == "") {
    let team1Result = "";
    let team2Result = "";

    if (battle.Team1Crowns > battle.Team2Crowns) {
      team1Result = "Winner";
      team2Result = "Loser";
    } else {
      team1Result = "Loser";
      team2Result = "Winner";
    }

    return (
      <div className={styles.battle}>

        <NavLink to={"/player/id/" + battle.Team1Id} >

          <h1>{battle.Team1Name}</h1>


          <p><b>{team1Result}</b></p>
          <p ><b>Crowns:</b>{battle.Team1Crowns}</p>
          <p><b>Trophies:</b>{battle.Team1StartingTrophies}<i>({((team1Result == "Winner") ? "+" + battle.Team1TrophyChange : battle.Team1TrophyChange)})</i></p>


          <Deck deck={battle.Team1DeckA} />


        </NavLink>

        <div id="center-panel" className={styles.centerPanel}>
          <h1>VS</h1>
          <Time time={battle.BattleTime} />
        </div>

        <NavLink to={"/player/id/" + battle.Team2Id} >
          <h1>{battle.Team2Name}</h1>
          <p ><b>{team2Result}</b></p>
          <p ><b>Crowns:</b> {battle.Team2Crowns}</p>
          <p><b>Trophies:</b>{battle.Team2StartingTrophies}<i>({((team2Result == "Winner") ? "+" + battle.Team2TrophyChange : battle.Team2TrophyChange)})</i></p>


          <Deck deck={battle.Team2DeckA} />
        </NavLink>
      </div>);
  }
}
export default memo(Battle);
