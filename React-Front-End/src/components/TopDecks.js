import React, { Component } from "react";
import Deck from "../components/Deck";
import { GetTopDecks } from "../Utilities/axios-functions";
import styles from "../cssModules/TopDecks.module.css"
import linkImg from "../assets/primary-copy.png"

class TopDecks extends Component {
  constructor(props) {
    super(props);
    this.state = {
      playerTag: "",
      topDecks: [],
      date: new Date(),
    };
    this.handleClick = this.handleClick.bind(this);
  }

  async componentDidMount() {
    const { playerTag } = this.props;
    this.setState({playerTag:playerTag});

    do {  
      //gets player's best decks from backend
      var decks = await GetTopDecks(playerTag);

      //if successfully fetched the decks are set to the state variable
      if (decks) this.setState({ topDecks: decks });

      if (this.state.topDecks.length == 0 ) {
        var timeElapsed = Date.now() - this.state.date;
        if (10000 > timeElapsed > 0) {
          await new Promise((r) => setTimeout(r, 10000 - timeElapsed));
        }
      }
      this.setState({ date: Date.now() });
    } while (this.state.topDecks.length == 0);
  }
  async componentDidUpdate() {
    const { playerTag } = this.props;

    //if there is a new tag in the header it will search that players top decks
    if (playerTag != this.state.playerTag && playerTag != undefined) {
      this.setState({ topDecks: [] });
      this.setState({ playerTag: playerTag });

      do {  
        //gets player's best decks from backend
        var decks = await GetTopDecks(playerTag);

        //if successfully fetched the decks are set to the state variable
        if (decks) this.setState({ topDecks: decks });

        if (this.state.topDecks.length == 0) {
          var timeElapsed = Date.now() - this.state.date;
          if (10000 > timeElapsed > 0) {
            await new Promise((r) => setTimeout(r, 10000 - timeElapsed));
          }
        }
        this.setState({ date: Date.now() });
      } while (this.state.topDecks.length == 0);
      
    }


  }


  handleClick = (d) =>{
    window.open(`https://link.clashroyale.com/deck/en?deck=${d.Card1Id};${d.Card2Id};${d.Card3Id};${d.Card4Id};${d.Card5Id};${d.Card6Id};${d.Card7Id};${d.Card8Id}`);
  }
   
  render() {
    const { topDecks } = this.props;

    let decksDraw = [];
    let componentHeader = "";

    if (this.state.topDecks.length > 0) {
      componentHeader = <h2>Best Decks</h2>;
      decksDraw = this.state.topDecks.map((d, i) => (
        <div className={styles.deck}>
        <Deck key={"$top-deck-" + i} deck={d} />
        <p className={styles.stats}><b>Wins:</b>{d.Wins} <b>Loss:</b>{d.Loss} <b>Win/Loss Rate:</b>{d.WinLossRate} 
        <img className={styles.link} src={linkImg} onClick={this.handleClick.bind(this,d)}/></p>
        
        </div>
      ));
    }
    else componentHeader = <h2>Loading Best Decks...</h2>;

return(<div className={styles.topDecks}>
      {componentHeader}
      {decksDraw}
    </div>
  );

  }
}

export default TopDecks;
