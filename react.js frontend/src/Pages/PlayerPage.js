import React, { useState, useContext, useEffect } from "react";
import { Redirect, useParams } from "react-router-dom";

import { UserContext } from "../UserContext";
import { axios } from "../axios";

import Battle from "../components/Battle";
import Player from "../components/Player";
import ChestCollection from "../components/ChestCollection";
import BattleCollection from "../components/BattleCollection";
import Deck from "../components/Deck";
import { GetDeckAsync } from "../Utilities/axios-functions";

const PlayerPage = () => {
  const { playerTag } = useParams();
  const { user, setUser } = useContext(UserContext);

  const [tag, setTag] = useState("");
  const [deck, setDeck] = useState("");

  //same as componentDidMount
  useEffect(async() => {
    if (playerTag != undefined) {
      setTag(playerTag);
    } else if (user && user.tag != "") {
      setTag(user.tag);
    } 

  }, []);
  
  if(tag)
  {
    
    return (
      <div>
      {<ChestCollection playerTag={tag} />}
        {<Player playerTag={tag} />}
        {<BattleCollection playerTag={tag} />}
      </div>
    );
  }
  else return(<h1>Loading...</h1>)

};

export default PlayerPage;
