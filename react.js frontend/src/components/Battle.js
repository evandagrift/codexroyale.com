import React, { Component } from "react";
import Deck from "./Deck";
import Time from "./Time";
import styles from "../cssModules/Battle.module.css";

class Battle extends Component {
  constructor(props) {
    super(props);
    this.state = {};
  }
  render() {
    const { battle } = this.props;
    
    //only rendering 1v1 Battles
    if (battle.Team1DeckBId == 0 && battle.Team1DeckAId != 0) {
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
        

        <div id="left-panel" className={styles.leftPanel}>

        <h1>{battle.Team1Name}</h1>
        
        
        <p><b>{team1Result}</b></p>
                  <p ><b>Crowns:</b>{battle.Team1Crowns}</p>
                  <p><b>Trophies:</b>{battle.Team1StartingTrophies}<i>({((team1Result == "Winner") ? "+"+battle.Team1TrophyChange : battle.Team1TrophyChange )})</i></p>
                  
                  
                  <Deck deck={battle.Team1DeckA} />


        </div>

        <div id="center-panel" className={styles.centerPanel}>
          <h1>VS</h1>
          <Time  time={battle.BattleTime}/>
        </div>

        <div id="right-panel" className={styles.rightPanel}>

        <h1>{battle.Team2Name}</h1>
        <p ><b>{team2Result}</b></p>
                  <p ><b>Crowns:</b> {battle.Team2Crowns}</p>
                  <p><b>Trophies:</b>{battle.Team2StartingTrophies}<i>({((team2Result == "Winner") ? "+"+battle.Team2TrophyChange : battle.Team2TrophyChange )})</i></p>
                  
                
                  <Deck deck={battle.Team2DeckA} />

        </div>
    </div>);
      
      
    }
    else return null;

  }
}
export default Battle;
