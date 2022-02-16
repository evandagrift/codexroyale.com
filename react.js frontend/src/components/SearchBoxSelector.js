import React, { Component } from "react";

class SearchBoxSelector extends Component {
  constructor(props) {
    super(props);
    this.state = {
      searching: "player",
      playerClass: "nav-link active",
      clanClass: "nav-link text-light ",
    };
  }

  render() {
    const clickPlayer = () =>
    {
        this.props.playerSearchSelect();
        this.setState({playerClass:"nav-link active", clanClass:"nav-link text-light", searching:"player"});
    }
    const clickClan = () =>
    {
        this.props.clanSearchSelect();
        this.setState({playerClass:"nav-link text-light", clanClass:"nav-link active", searching:"clan"});
    }

    let searchSelect = (
      <div>
            <ul className="nav nav-tabs border-0">
              <li className="nav-item">
                <button key="$playerButton" className={this.state.playerClass} onClick={clickPlayer}>Player</button>
              </li>

              <li className="nav-item">
                <button key="$clanButton" className={this.state.clanClass} onClick={clickClan}>Clan</button>
              </li>
            </ul>
      </div>
    );
    return <div>{searchSelect}</div>;
  }
}
export default SearchBoxSelector;
