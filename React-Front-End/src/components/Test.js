import { Alert } from 'bootstrap';
import React, { Component, useEffect } from 'react';
import Deck from './Deck';
import { GetDeckAsync } from "../Utilities/axios-functions";

class Test extends Component{
  constructor(props){
    super(props);
    this.state = {
      deck:""
    };
  } 

  async componentDidMount()
  {
        try {
        let tempDeck = await GetDeckAsync();
        this.setState({deck:tempDeck});
        }
        catch{}
  }

  render () {
    let draw = '';
    if(this.state.deck)
    {
        draw = <Deck deck={this.state.deck} />;
    }
    else{
      return (<h1>Loading</h1>)
    }

    return (<div>
        {draw}
        </div>
    );
  }

}
export default Test;  