import React, { useState, useContext, useEffect } from "react";
import { Redirect, useParams } from "react-router-dom";

import { UserContext } from "../UserContext";
import Clan from "../components/Clan";
import { GetClanAsync } from "../Utilities/scripts";

const ClanPage = () => {

    const { user, setUser } = useContext(UserContext);
    const [tag, setTag] = useState('');
    const [redirect, setRedirect] = useState(false);
    const {clanTag} = useParams();

  //same as componentDidMount
  useEffect( () => {
    if(clanTag != undefined) { setTag(clanTag) }
    else if(user && user.clanTag != "") { setTag(user.clanTag) }
    else setRedirect(true);
  }, [] );



    let draw =(<h1>Loading...</h1>);

    const clan = <Clan clanTag={tag} />;

  if(clan)
  {
     if(tag) return clan;
    else return draw;
    
  }

    
};

export default ClanPage;