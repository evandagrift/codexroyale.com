import { event } from "jquery";
import React, { Component, PureComponent } from "react";
import styles from "../../cssModules/Deck.module.css"
import EditableCard from "../warDayDeckEditing/EditableCard";

class EditableDeck extends PureComponent {
    constructor(props) {
        super(props);
        this.state = {};
    }
    // async componentDidMount() {
    //   try {
    //   }
    //   catch { }
    // }

    render() {
        const { deck, deckNumber } = this.props;
        if (deck != undefined) {
            if (Object.keys(deck).length > 0) {
                if (true) {
                    let dragEvent = (e, card) => { this.props.dragEvent(e, card, `deck${deckNumber}`); }
                    let dropEvent = (e, card) => { this.props.dropEvent(e, card, `deck${deckNumber}`); }
                    let dragOver = (e) => { this.props.dragOver(e); }

                    return (
                        <div onDragOver={dragOver} className={styles.deck}>
                            <div onDragOver={dragOver} className={styles.row}>
                                <EditableCard dragOver={dragOver} dragEvent={dragEvent} dropEvent={dropEvent} key={"$id" + deck.Card1Id} card={deck.Card1} />
                                <EditableCard dragOver={dragOver} dragEvent={dragEvent} dropEvent={dropEvent} key={"$id" + deck.Card2Id} card={deck.Card2} />
                                <EditableCard dragOver={dragOver} dragEvent={dragEvent} dropEvent={dropEvent} key={"$id" + deck.Card3Id} card={deck.Card3} />
                                <EditableCard dragOver={dragOver} dragEvent={dragEvent} dropEvent={dropEvent} key={"$id" + deck.Card4Id} card={deck.Card4} />
                            </div>
                            <div className={styles.row}>
                                <EditableCard dragOver={dragOver} dragEvent={dragEvent} dropEvent={dropEvent} key={"$id" + deck.Card5Id} card={deck.Card5} />
                                <EditableCard dragOver={dragOver} dragEvent={dragEvent} dropEvent={dropEvent} key={"$id" + deck.Card6Id} card={deck.Card6} />
                                <EditableCard dragOver={dragOver} dragEvent={dragEvent} dropEvent={dropEvent} key={"$id" + deck.Card7Id} card={deck.Card7} />
                                <EditableCard dragOver={dragOver} dragEvent={dragEvent} dropEvent={dropEvent} key={"$id" + deck.Card8Id} card={deck.Card8} />
                            </div>
                        </div>
                    );
                }

            }
        }
        return <div></div>;

    }

}


export default EditableDeck;

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
