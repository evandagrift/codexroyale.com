import React, { Component } from 'react';
import styles from "../../cssModules/Card.module.css";

class EditableCard extends Component{
  constructor(props){
    super(props);
  } 
  render () {
          
    const { card } = this.props;

    let dragEvent = (e) =>{
        this.props.dragEvent(e,card);
    }
    let dropEvent = (e) =>{
        this.props.dropEvent(e,card);
    }
    let dragOver = (e) =>{
      this.props.dragOver(e,card);
    }
    if(card) return (<div onDrop={dropEvent} onDragOver={dragOver} onDrag={dragEvent} className={styles.card}><img  key={"$id-"+card.Id} src={card.Url}/></div>);
    
    else return (<div/>);
  }

}
export default EditableCard;  