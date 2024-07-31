import React, { Component } from "react";
import Battle from "./Battle";
import { GetBattlesAsync, GetPlayerBattlesAsync } from "../Utilities/axios-functions";
import styles from "../cssModules/BattleCollection.module.css";

class BattleCollection extends Component {
  constructor(props) {
    super(props);
    this.state = {

    };
  }

  render() {
    const { handleScroll, battles, playerTag } = this.props;
    let componentHeader = <h2>Recently Recorded Battles</h2>;
    let battlesDraw = undefined;

    if (battles && battles.length > 0) {
      componentHeader = <h2>Recently Recorded Battles</h2>;
      battlesDraw = battles.map((b, i) => (
        <Battle key={`battle-${i}`} battle={b} />
      ));
    }

    return (
      <div className={styles.battleCollection} onScroll={handleScroll}>
        {componentHeader}

        <div className={styles.battleCollection}>
          {battlesDraw}
        </div>
      </div>
    );
  }
}

export default BattleCollection;
