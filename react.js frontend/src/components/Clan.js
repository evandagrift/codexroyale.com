import React, { Component } from "react";
import { Redirect } from "react-router";

import Card from "../components/Card";
import Deck from "../components/Deck";
import { GetClanAsync } from "../Utilities/axios-functions";
import ClanPlayer from "./ClanPlayer";
import ClanPlayerCollection from "./ClanPlayerCollection";
import Time from "./Time";

class Clan extends Component {
  constructor(props) {
    super(props);
    this.state = {
      clan: [],
      redirect: false
    };
  }

  async componentDidMount() {
    const { clanTag } = this.props;

    if (clanTag) {
      const clan = await GetClanAsync(clanTag);

      if (clan != undefined) {

        this.setState({ clan: clan });
      } 
      else {
        this.setState({redirect:true});
      }
    }
  }

  render() {
    let clanDraw = "";
    let membersDraw = "";
    let membersHeader = "";

    if (this.state.redirect)
    {
      return (<Redirect to="/" />);
    }
    if (this.state.clan && this.state.clan.Name) {
      clanDraw = (
        <div className=" m-0">
          <h1>{this.state.clan.Name}</h1>

          <h6>
            <b>Tag:</b>
            {this.state.clan.Tag}
          </h6>
          <p>
            <b>Members:</b>
            {this.state.clan.Members}/50
          </p>
          <p>
            <b>{this.state.clan.Description}</b>
          </p>

          <p>
            <b>Required Trophies:</b>
            {this.state.clan.RequiredTrophies}
          </p>

          <div className="col ">
            <p>
              <b>Donations Per Week:</b>
              {this.state.clan.DonationsPerWeek}
            </p>
            <p>
              <b>Clan War Trophies:</b>
              {this.state.clan.ClanWarTrophies}
            </p>
            <p>
              <b>Clan Chest Level:</b>
              {this.state.clan.ClanChestLevel}
            </p>
            <p>
              <b>Clan Chest Status:</b>
              {this.state.clan.ClanChestStatus}
            </p>
          </div>

          <div className="col">
            <p>
              <b>Type:</b>
              {this.state.clan.Type}
            </p>
            <p>
              <b>Location Code:</b>
              {this.state.clan.LocationCode}
            </p>

            <p>
              <b>Badge Id:</b>
              {this.state.clan.BadgeId}
            </p>
            <p>
              <b>Clan Score:</b>
              {this.state.clan.ClanScore}
            </p>
          </div>
        </div>
      );
    } else return <h1>Loading Clan Data, This May Take Some Time</h1>;


    
    if (this.state.clan && this.state.clan.MemberList) {
      membersDraw = (
        <ClanPlayerCollection
          clanPlayerCollection={this.state.clan.MemberList}
        />
      );
    }

    return (
      <div>
          {clanDraw}
          {membersDraw}
      </div>
    );
  }
}
export default Clan;
