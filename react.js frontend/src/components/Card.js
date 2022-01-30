import React, { Component } from 'react';
import styles from "../cssModules/Card.module.css";

class Card extends Component{
  constructor(props){
    super(props);
  } 
  render () {
          
    const { card } = this.props;

    if(card) return (<div className={styles.card}><img key={"$id-"+card.Id} src={card.Url}/></div>);
    
    else return (<div/>);
  }

}
export default Card;  