import { stat } from "fs";
import React, { Component, useState } from "react";
import { Redirect, Route } from "react-router";
import { FormatTag } from "../Utilities/scripts";
import SearchBoxSelector from "./SearchBoxSelector";

class SearchBox extends Component {
  constructor(props) {
    super(props);
    this.state = {
      searching: "player",
      searchInput: "",
      validTag: "false",
      searchPlaceholder:"#29PGJURQL",
      playerClass:"nav-link active",
      clanClass:"nav-link",
      redirect:false
    };
  }

  render() {

    let draw = '';

const playerSearchSelect = e => this.setState({searching:"player", searchPlaceholder:"#29PGJURQL"});
const clanSearchSelect = e => this.setState({searching:"clan", searchPlaceholder:"#8CYPL8R"});

    //      homeSplash = (<Redirect push to={redirectUrl}/>);


//"/player/" + FormatTag(searchString)

const search = (e) => {
    this.setState({searchInput:FormatTag(this.state.searchInput),redirect:true});

//this.props.search("/" + this.state.searching + "/"+ this.state.searchInput);

// if(this.state.searching == "player") {
//     return (<div>< push to="/Player/%2329PGJURQL" /></div>);
//     //return (<div><Redirect exact to={"/Player/" + FormatTag(this.state.searchInput)} /></div>);
// }
// else if(this.state.searching == "clan"){
//     return (<div><Redirect exact to={"/Clan/" + FormatTag(this.state.searchInput)} /></div>);
// }

}
let searchGroup = (<form key="$playerForm" onSubmit={this.search}>
    
<input key="$search-input"  onChange={(e) => this.setState({searchInput:e.target.value})} className="form-control"  placeholder={this.state.searchPlaceholder} />
<button key="$search-button" className="btn-dark mt-2 mb-3" onClick={search}>Get {this.state.searching} Data</button>
</form>);

if(!this.state.redirect)
{
    draw = (<div className={"home-search"}> 
      <h2>Search For Player or Clan</h2>
       
            <p><SearchBoxSelector playerSearchSelect={playerSearchSelect} clanSearchSelect={clanSearchSelect} />
            {searchGroup}
            </p>
        </div>);

}
else draw = <Redirect push to={"/" + this.state.searching + "/" + this.state.searchInput}/>

    return draw;
  }
}
export default SearchBox;
