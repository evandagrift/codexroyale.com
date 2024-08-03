import { Alert } from 'bootstrap';
import React, { Component, useEffect } from 'react';
import EditableDeck from './EditableDeck';
import TestCard from './TestCard';
import { GetDeckAsync } from "../Utilities/axios-functions";
import { getAllCards } from "../Utilities/axios-functions";

class Test extends Component {
  constructor(props) {
    super(props);
    this.state = {
      deck: {},
      cards:[],
      card: {
        "Id": 26000004,
        "Name": "P.E.K.K.A",
        "Url": "https://api-assets.clashroyale.com/cards/300/MlArURKhn_zWAZY-Xj1qIRKLVKquarG25BXDjUQajNs.png"
      }
    };
  }

  async componentDidMount() {
    try {
      let tempDeck = await GetDeckAsync();
      let tempCards = await getAllCards();
        console.log(tempCards)
      this.setState({ deck: tempDeck });
      this.setState({ cards: tempCards });
      console.log(tempCards);
    }
    catch { }
  }

  render() {
    let draw = '';
    let cards = '';
    const dragEvent = (e,card) => { this.setState({card:card}) }

    const dropEvent = (e, card) => {
      console.log(card);
      
      console.log(e);
      let tempDeck = this.state.deck;
      let x = 1;
      let tempCards = this.state.cards;
      let index = tempCards.findIndex(c => c.Id == card.Id)

      switch (card.Id) {
        case tempDeck.Card1.Id:
          this.setState({card:tempDeck.Card1})
          tempDeck.Card1 = card;
          break;
        case tempDeck.Card2.Id:
          this.setState({card:tempDeck.Card2})
          tempDeck.Card2 = card;
          break;
        case tempDeck.Card3.Id:
          this.setState({card:tempDeck.Card3})
          tempDeck.Card3 = card;
          break;
        case tempDeck.Card4.Id:
          this.setState({card:tempDeck.Card4})
          tempDeck.Card4 = card;
          break;
        case tempDeck.Card5.Id:
          this.setState({card:tempDeck.Card5})
          tempDeck.Card5 = card;
          break;
        case tempDeck.Card6.Id:
          this.setState({card:tempDeck.Card6})
          tempDeck.Card6 = card;
          break;
        case tempDeck.Card7.Id:
          this.setState({card:tempDeck.Card7})
          tempDeck.Card7 = card;
          break;
        case tempDeck.Card8.Id:
          this.setState({card:tempDeck.Card8})
          tempDeck.Card8 = card;
          break;
      }
      this.setState({ deck: tempDeck })

    }

    const handleDragOver = e => { e.preventDefault(); }

    if (this.state.deck) {
      
      draw = (<div>{<EditableDeck dragEvent={dragEvent} dragOver={handleDragOver} dropEvent={dropEvent} deck={this.state.deck} />}{<TestCard dragOver={handleDragOver} key={"$card-" + this.state.card.Id} dragEvent={dragEvent} dropEvent={dropEvent} card={this.state.card} />}</div>);
      cards = this.state.cards.map((b, i) => (
        <TestCard key={"$card-" + i} dragOver={handleDragOver} dragEvent={dragEvent} dropEvent={dropEvent} card={b} />
      ));
    }
    else {
      return (<h1>Loading</h1>)
    }

    return (<div>
      {draw}
      {cards}
    </div>
    );
  }

}
export default Test;  