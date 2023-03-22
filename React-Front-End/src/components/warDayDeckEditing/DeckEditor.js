import { Alert } from 'bootstrap';
import React, { Component, useEffect } from 'react';
import EditableDeck from './EditableDeck';
import EditableCard from './EditableCard';
import { GetDeckAsync } from "../../Utilities/axios-functions";
import { getAllCards } from "../../Utilities/axios-functions";

class DeckEditor extends Component {
  constructor(props) {
    super(props);
    this.state = {
      locationValues:["deck0","deck2","deck3","deck4","tray","cards"],
      cardDraggedFromLocation:"",
      emptyDeck: {},
      decksBeingEdited: [],
      cards: [],
      cardBeingMoved: {},
      cardToReplace: {},
      deckIds:[]
    };
  }

  async componentDidMount() {
    try {
      const tempDeck = await GetDeckAsync();
      const newDeck = await GetDeckAsync();
      const tempCards = await getAllCards();
      
      this.setState({ deck: tempDeck });
      this.setState({ cards: tempCards });
      let deckOne = tempDeck;
      let deckTwo = newDeck;
      deckOne.deckNumber = 0;
      deckTwo.deckNumber = 1;
      const decks = [deckOne, deckTwo];
      this.setState({ decks: decks });
    }
    catch { }
  }

  render() {
    let draw = '';
    let cards = '';

    const dragEvent = (e, card, location) => {
      this.setState({ cardBeingMoved: card });
      this.setState({ cardDraggedFromLocation: location});
    }

    const dropEvent = (e, card, location) => {

      console.log("dropping onto:"+location)
      console.log("dragging from:"+this.state.cardDraggedFromLocation)
      if (location != undefined && location != "") {
        switch (location) {
          case "deck0":
            case "deck1":
              case "deck2":
                case "deck3":
                  let deckEdited = this.state.decks[location[location.length-1]];
          
                  Object.keys(deckEdited).forEach(key => {
                    
                    if(deckEdited[key].Id == card.Id)
                    {
                      let decksToUpdate = this.state.decks;
                      deckEdited[key] = this.state.cardBeingMoved;
                      decksToUpdate[location[location.length-1]] = deckEdited;
                      console.log(decksToUpdate)
                      this.setState({decks:decksToUpdate})
                    }
                  });
                  break;
                  case "tray":
                    break;
                    case "cards":
                      break;
        }


          // console.log(deckEdited)
        // deckEdited.map((b, i) => {
        //   console.log(b)
        //   console.log(b)
        // });


        this.setState({ cardToReplace: card });






        // let x = 1;
        // let tempCards = this.state.cards;
        // let index = tempCards.findIndex(c => c.Id == card.Id)

        // switch (card.Id) {
        //   case tempDeck.Card1.Id:
        //     this.setState({ card: tempDeck.Card1 })
        //     tempDeck.Card1 = card;
        //     break;
        //   case tempDeck.Card2.Id:
        //     this.setState({ card: tempDeck.Card2 })
        //     tempDeck.Card2 = card;
        //     break;
        //   case tempDeck.Card3.Id:
        //     this.setState({ card: tempDeck.Card3 })
        //     tempDeck.Card3 = card;
        //     break;
        //   case tempDeck.Card4.Id:
        //     this.setState({ card: tempDeck.Card4 })
        //     tempDeck.Card4 = card;
        //     break;
        //   case tempDeck.Card5.Id:
        //     this.setState({ card: tempDeck.Card5 })
        //     tempDeck.Card5 = card;
        //     break;
        //   case tempDeck.Card6.Id:
        //     this.setState({ card: tempDeck.Card6 })
        //     tempDeck.Card6 = card;
        //     break;
        //   case tempDeck.Card7.Id:
        //     this.setState({ card: tempDeck.Card7 })
        //     tempDeck.Card7 = card;
        //     break;
        //   case tempDeck.Card8.Id:
        //     this.setState({ card: tempDeck.Card8 })
        //     tempDeck.Card8 = card;
        //     break;
        // }

      }

    }

    const handleDragOver = (e) => {
      e.preventDefault();
      // console.log(card);
    }

    if (this.state.decks) {

      draw = (<div>{<EditableDeck key={"$deck-" + 0} dragEvent={dragEvent} dragOver={handleDragOver} dropEvent={dropEvent} deck={this.state.decks[0]} deckNumber={0} />}{<EditableDeck key={"$card-" + 1} dragEvent={dragEvent} dragOver={handleDragOver} dropEvent={dropEvent} deck={this.state.decks[1]}  deckNumber={1}/>}</div>);
      cards = this.state.cards.map((b, i) => (
        <EditableCard key={"$card-" + i} dragOver={handleDragOver} dragEvent={dragEvent} dropEvent={dropEvent} card={b} />
      ));
    }
    else {
      return (<h1>Loading</h1>);
    }

    return (<div>
      {draw}
      {cards}
    </div>
    );
  }

}
export default DeckEditor;  