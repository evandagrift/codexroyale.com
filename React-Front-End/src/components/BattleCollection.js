import React, { memo } from "react";
import Battle from "./Battle";
import styles from "../cssModules/BattleCollection.module.css";

const BattleCollection = ({ battles }) => {
  const header = battles && battles.length > 0
    ? "Recently Recorded Battles"
    : "Recently Recorded Battles";

  return (
    <div className={styles.battleCollection}>
      <h2 className={styles.battleHeader}>{header}</h2>
      {battles && battles.length > 0 ? (
        <div className={styles.battleList}>
          {battles.map((b, i) => (
            <Battle key={`battle-${i}`} battle={b} />
          ))}
        </div>
      ) : (
        <div className={styles.emptyState}>No battles available yet.</div>
      )}
    </div>
  );
};

export default memo(BattleCollection);
