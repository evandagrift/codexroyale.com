import { Alert } from 'bootstrap';
import React, { Component, useEffect } from 'react';
import Chest from './Chest';
import { GetChestsAsync } from '../Utilities/axios-functions';
import { FormatTag } from '../Utilities/scripts';
import styles from '../cssModules/ChestCollection.module.css';

class ChestCollection extends Component{
  constructor(props){
    super(props);
    this.state = {
      chestCollection:[]
    };
  } 

  async componentDidMount()
  {
    const { playerTag } = this.props;
      if(playerTag)
      {
        try {
        let chests = await GetChestsAsync(FormatTag(playerTag));
        this.setState({chestCollection:chests});
        }
        catch{}
      }
  }



  render () {
    let draw = '';
    let header = ( <h2>Loading Upcoming Chests</h2> );
    if(this.state.chestCollection && this.state.chestCollection.length > 0)
    {
      header = (<div className=""><h2>Upcoming Chests</h2> <p>(you need x number of wins to gain the below chests)</p></div>);
      draw = this.state.chestCollection.map((c)=> <Chest key={"$chest-+"+c.Index} chest={c}/>)
    }
    return (<div className={styles.collection} >
      {header}
      <div className={styles.chestbox} key="$chest-collection">{draw}</div>
       </div>
    );
  }

}
export default ChestCollection;  