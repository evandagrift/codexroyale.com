import React, { Component } from 'react';
import { ConvertTimer } from "../Utilities/scripts";

class Time extends Component{
  constructor(props){
    super(props);
    this.state = {};
  } 
  render () {
          
    const { time } = this.props;
    let draw = '';
    if(time)
    {
      let convertedDate = ConvertTimer(time);
        draw = (convertedDate);

    }
    
    
    return (
       <h4>{draw}</h4>
    );
  }

}
export default Time;  