import React, { Component, useEffect } from "react";
import Battle from "./Battle";
import { GetBattlesAsync } from "../Utilities/axios-functions";
import { axios } from "../axios";
import { GetPlayerBattlesAsync } from "../Utilities/axios-functions";
import styles from "../cssModules/BattleCollection.module.css";

class BattleCollection extends Component {
  constructor(props) {
    super(props);
    this.state = {
      playerTag: "",
      battles: [],
      date: new Date(),
    };
  }

  async componentDidMount() {
    const { playerTag } = this.props;

    this.setState({ date: Date.now() });

    do {
      //if there is a tag in the header it will search that individual player
      if (playerTag) {
        this.setState({ playerTag: playerTag });
        //gets player's battles from backend
        const fetchedBattles = await GetPlayerBattlesAsync(playerTag);

        //if successfully fetched battles sets the state variable
        if (fetchedBattles) this.setState({ battles: fetchedBattles });
      } //if this collection is being creeated without a given tag fetches all recent battles from backend
      else {
        //gets recent battles from backed
        const fetchedBattles = await GetBattlesAsync();
        if (fetchedBattles) this.setState({ battles: fetchedBattles });
      }

      if (this.state.battles.length < 10) {
        var timeElapsed = Date.now() - this.state.date;
        if (7500 > timeElapsed > 0) {
          await new Promise((r) => setTimeout(r, 7500 - timeElapsed));
        }
        
        this.setState({ date: Date.now() });
      }
    } while (this.state.battles.length < 10);
  }

  async componentDidUpdate() {
    const { playerTag } = this.props;

    //if there is a tag in the header it will search that individual player
    if (playerTag != this.state.playerTag && playerTag != undefined) {
      this.setState({ battles: [] });
      this.setState({ playerTag: playerTag });

      do {  
        //gets player's battles from backend
        const fetchedBattles = await GetPlayerBattlesAsync(playerTag);

        //if successfully fetched battles sets the state variable
        this.setState({ battles: fetchedBattles });

        if (this.state.battles.length < 10) {
          var timeElapsed = Date.now() - this.state.date;
          if (10000 > timeElapsed > 0) {
            await new Promise((r) => setTimeout(r, 10000 - timeElapsed));
          }
        }
        this.setState({ date: Date.now() });
      } while (this.state.battles.length < 10);
      
    }

    // if(timeElapsed)
  }

  render() {
    let componentHeader = <h2>Loading Battles...</h2>;

    let battlesDraw = [];

    //if the battles state was set after success fully fetching from backend, it maps these battles as components into battlesDraw[]
    if (this.state.battles.length > 0) {
      battlesDraw = this.state.battles.map((b, i) => (
        <Battle key={"$battle-" + i} battle={b} />
      ));
    }

    if (battlesDraw.length > 0) {
      componentHeader = <h2>Recently Recorded Battles</h2>;
    }

    return (
      <div className={styles.battleCollection}>
        {componentHeader}
        {battlesDraw}
      </div>
    );
  }
}
export default BattleCollection;
