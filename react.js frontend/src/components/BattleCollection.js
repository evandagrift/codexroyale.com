import React, { Component, useEffect } from 'react';
import Battle from './Battle';
import { GetBattlesAsync } from "../Utilities/axios-functions";
import { axios } from "../axios";
import { GetPlayerBattlesAsync } from "../Utilities/axios-functions"; 
import styles from "../cssModules/BattleCollection.module.css";

class BattleCollection extends Component{
  constructor(props){
    super(props);
    this.state = {
      battles:[]
    };
  } 

  async componentDidMount()
  {
    const { playerTag } = this.props;

    //if there is a tag in the header it will search that individual player
    if(playerTag)
    {
      //gets player's battles from backend
      const fetchedBattles = await GetPlayerBattlesAsync(playerTag);

      //if successfully fetched battles sets the state variable
      if(fetchedBattles) this.setState({battles:fetchedBattles});
    }
    else //if this collection is being creeated without a given tag fetches all recent battles from backend
    {
      //gets recent battles from backed
      const fetchedBattles = await GetBattlesAsync();
      if(fetchedBattles) this.setState({battles:fetchedBattles});
    }
  }


  render () {
    let componentHeader = (<h2>Loading Battles...</h2>);

    let battlesDraw =  [];



    //if the battles state was set after success fully fetching from backend, it maps these battles as components into battlesDraw[]
    if(this.state.battles) 
    {
      battlesDraw = this.state.battles.map((b,i) => <Battle key={'$battle-'+i} battle={b}/>);
    }

    if(battlesDraw.length > 0) { componentHeader = (<h2>Recently Recorded Battles</h2>); }
    

    return (<div className={styles.battleCollection} >
      {componentHeader}
      {battlesDraw}
       </div>
    );
  }

}
export default BattleCollection;  