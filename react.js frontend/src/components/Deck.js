import React, { Component} from "react";
import Card from "../components/Card";
import styles from "../cssModules/Deck.module.css"

class Deck extends Component {
  constructor(props) {
    super(props);
    this.state = {};
  }

  render() {
    const { deck } = this.props;
    if(deck)
    {
 return (
        <div className={styles.deck}>
          <div className={styles.row}>
            <Card key={"$id" + deck.Card1Id} card={deck.Card1} />
            <Card key={"$id" + deck.Card2Id} card={deck.Card2} />
            <Card key={"$id" + deck.Card3Id} card={deck.Card3} />
            <Card key={"$id" + deck.Card4Id} card={deck.Card4} />
          </div>

          <div className={styles.row}>
            <Card key={"$id" + deck.Card5Id} card={deck.Card5} />
            <Card key={"$id" + deck.Card6Id} card={deck.Card6} />
            <Card key={"$id" + deck.Card7Id} card={deck.Card7} />
            <Card key={"$id" + deck.Card8Id} card={deck.Card8} />
          </div>
        </div>
      );
    }
    else return (<div></div>);
  
  }
    
}


export default Deck;

/*
some old code



    if(!deck.Card1) {return (<div>loading</div>)}
      else return(<div className="deck">
        <div className="d-inline-flex m-0">
        <Card  key={'$id'+deck.Card1.Id} card={deck.Card1} />
        <Card key={'$id'+deck.Card2.Id} card={deck.Card2} />
        <Card  key={'$id'+deck.Card3.Id} card={deck.Card3} />
        <Card key={'$id'+deck.Card4.Id} card={deck.Card4} />
        </div>
        
        <div className="d-inline-flex m-0">
           <Card key={'$id'+deck.Card5.Id} card={deck.Card5} />
           <Card key={'$id'+deck.Card6.Id} card={deck.Card6} />
           <Card key={'$id'+deck.Card7.Id} card={deck.Card7} />
           <Card key={'$id'+deck.Card8.Id} card={deck.Card8} />
           </div>

        </div>);
        */
