import React, { Component } from 'react';
import styles from "../cssModules/Chest.module.css";
class Chest extends Component{
  constructor(props){
    super(props);
    this.state = {};
  } 


  
  render () {
          
    const { chest } = this.props;

    if(chest.Name)
    {
      return (<div className={styles.chest}><strong>{chest.Index+1}</strong> <img  key={'$id'+chest.Index}  src={chest.Url}/>
     <p>{chest.Name}</p></div>);
    }
  }

}
export default Chest;  