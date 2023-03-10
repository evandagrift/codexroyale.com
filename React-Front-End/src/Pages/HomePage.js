import React, { useContext } from "react";
import { UserContext } from "../UserContext";
import BattleCollection from "../components/BattleCollection";
import SearchBox from "../components/SearchBox";
import ChestCollection from "../components/ChestCollection";
import styles from "../cssModules/HomePage.module.css";

const HomePage = () => {
  const { user, setUser } = useContext(UserContext);

  let upcomingChests = '';
  let drawBattles = <BattleCollection />;
  let searchBox = <SearchBox/>;
  let greeting = '';

  if(user) 
  { 
    upcomingChests =(<div className={styles.chestCollection}> <ChestCollection playerTag={user.tag} /></div>); 
  greeting = (<div className={styles.greeting}><h1>Welcome {user.username}</h1></div>)
  }

  return (
    <div>
    <div className={styles.homePage}>
      <img className={styles.homeImgTemp} src={require("../assets/KeyArt_Season_011.png")} /> 
      <div className={styles.overlayTemp}>
      {greeting}
      {upcomingChests}
      {searchBox}
    </div>
    </div>
      {drawBattles}
    </div>
  );
};

export default HomePage;

/*
 <div>
      <div className={styles.backgroundImage}>
      {greeting}
      {upcomingChests}
      {searchBox}
      </div>
      {drawBattles}
    </div>
    */