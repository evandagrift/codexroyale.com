import React, { Component } from "react";
import Deck from "./Deck";
import Time from "./Time";
import styles from "../cssModules/Battle.module.css";
import { GetPlayerTagAsync } from "../Utilities/axios-functions";

class Battle extends Component {
  render() {
    const { battle } = this.props;

    var clickPlayer1 = async () => {
      var playerTag = await GetPlayerTagAsync(battle.Team1Id);
      console.log(playerTag);
    }

    var clickPlayer2 = async () => {
      var playerTag = await GetPlayerTagAsync(battle.Team2Id);
      console.log(playerTag);
    }

    const isTeam1Win = battle.Team1Win;
    const crownDisplay = `${battle.Team1Crowns} - ${battle.Team2Crowns}`;
    const gameMode = battle.GameMode?.Name || "Ranked";

    return (
      <div className={`${styles.battle} ${isTeam1Win ? styles.win : styles.loss}`}>
        <div className={styles.battleHeader}>
          <div className={styles.resultBadge}>
            {isTeam1Win ? "Victory" : "Defeat"}
          </div>
          <div className={styles.headerCenter}>
            <div className={styles.gameMode}>{gameMode}</div>
            <div className={styles.crownScore}>
              <span className={styles.crownBlue}>👑</span>
              {crownDisplay}
              <span className={styles.crownRed}>👑</span>
            </div>
          </div>
        </div>

        <div className={styles.teamContainer}>
          <div className={styles.teamPanel} onClick={clickPlayer1}>
            <div className={styles.teamHeader}>
              <div className={styles.playerName}>{battle.Team1Name}</div>
              <div className={`${styles.trophyChange} ${isTeam1Win ? styles.positive : styles.negative}`}>
                {battle.Team1StartingTrophies}
                <span className={styles.trophyDelta}>
                  {battle.Team1TrophyChange >= 0 ? "+" : ""}{battle.Team1TrophyChange}
                </span>
              </div>
            </div>
            <Deck deck={battle.Team1DeckA} />
            <div className={styles.hpBar}>
              <div className={styles.hpItem}>
                <span className={styles.hpLabel}>King</span>
                <div className={styles.hpValue}>
                  {battle.Team1KingTowerHp === -1 ? "Full" : battle.Team1KingTowerHp}
                </div>
              </div>
              <div className={styles.hpItem}>
                <span className={styles.hpLabel}>L</span>
                <div className={styles.hpValue}>
                  {battle.Team1PrincessTowerHpA === -1 ? "Full" : battle.Team1PrincessTowerHpA}
                </div>
              </div>
              <div className={styles.hpItem}>
                <span className={styles.hpLabel}>R</span>
                <div className={styles.hpValue}>
                  {battle.Team1PrincessTowerHpB === -1 ? "Full" : battle.Team1PrincessTowerHpB}
                </div>
              </div>
            </div>
          </div>

          <div className={styles.vsDivider}>VS</div>

          <div className={styles.teamPanel} onClick={clickPlayer2}>
            <div className={styles.teamHeader}>
              <div className={styles.playerName}>{battle.Team2Name}</div>
              <div className={`${styles.trophyChange} ${!isTeam1Win ? styles.positive : styles.negative}`}>
                {battle.Team2StartingTrophies}
                <span className={styles.trophyDelta}>
                  {battle.Team2TrophyChange >= 0 ? "+" : ""}{battle.Team2TrophyChange}
                </span>
              </div>
            </div>
            <Deck deck={battle.Team2DeckA} />
            <div className={styles.hpBar}>
              <div className={styles.hpItem}>
                <span className={styles.hpLabel}>King</span>
                <div className={styles.hpValue}>
                  {battle.Team2KingTowerHp === -1 ? "Full" : battle.Team2KingTowerHp}
                </div>
              </div>
              <div className={styles.hpItem}>
                <span className={styles.hpLabel}>L</span>
                <div className={styles.hpValue}>
                  {battle.Team2PrincessTowerHpA === -1 ? "Full" : battle.Team2PrincessTowerHpA}
                </div>
              </div>
              <div className={styles.hpItem}>
                <span className={styles.hpLabel}>R</span>
                <div className={styles.hpValue}>
                  {battle.Team2PrincessTowerHpB === -1 ? "Full" : battle.Team2PrincessTowerHpB}
                </div>
              </div>
            </div>
          </div>
        </div>

        <div className={styles.battleFooter}>
          <Time time={battle.BattleTime} />
        </div>
      </div>
    );
  }
}

export default Battle;
