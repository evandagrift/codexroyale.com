import { useState, useContext, useEffect } from "react";
import { useParams } from "react-router-dom";

import { UserContext } from "../UserContext";
import Player from "../components/Player";
import ChestCollection from "../components/ChestCollection";
import BattleCollection from "../components/BattleCollection";
import TopDecks from "../components/TopDecks";

const PlayerPage = () => {
  const { playerTag, teamId } = useParams();
  // const { user, setUser } = useContext(UserContext);

  //same as componentDidMount
  useEffect(() => {
  }, [playerTag, teamId]);
  
  if (playerTag) {
    return (
      <div>
        {/* {<ChestCollection playerTag={playerTag} />}
        {<Player playerTag={playerTag} />}
        {<TopDecks playerTag={playerTag} />}
        {<BattleCollection playerTag={playerTag} />} */}
      </div>
    );
  }
  else if (teamId){
    return (
      <div>
        {/* {<ChestCollection playerTag={teamId} />} */}
        {<Player teamId={teamId} />}
        {/* {<TopDecks teamId={teamId} />}
        {<BattleCollection teamId={teamId} />} */}
      </div>
    );
  }
  else return (<h1>Loading...</h1>);

};

export default PlayerPage;
