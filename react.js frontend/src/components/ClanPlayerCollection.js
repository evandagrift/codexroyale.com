import React, { Component } from "react";
import ClanPlayer from "./ClanPlayer";

class ClanPlayerCollection extends Component {
  constructor(props) {
    super(props);
    this.state = {};
  }
  render() {
    const { clanPlayerCollection } = this.props;
    let draw = "";


    if (clanPlayerCollection != null) {
      draw = clanPlayerCollection.map((c) => (
        <ClanPlayer key={"$player-list-" + c.Tag} clanPlayer={c} />
      ));
    }


    return (
      <div className="table-responsive">
        <table key="$player-collection" className="table table-hover">
          <thead>
            <tr>
              <th scope="col"> Name</th>
              <th scope="col"> Tag</th>
              <th scope="col"> Level </th>
              <th scope="col"> Trophies </th>
              <th scope="col"> Role </th>
              <th scope="col"> Last Seen </th>
              <th scope="col"> Donated </th>
              <th scope="col"> Donations Recieved </th>
            </tr>
          </thead>
          <tbody>
          {draw}
          </tbody>

        </table>
        </div>
    );
  }
}
export default ClanPlayerCollection;

{
  /* <div className="card-body text-center d-inline-block">
<div className="col-2 clan-list-item"><b>Name</b></div>
<div className="col-2 clan-list-item"><b>Tag</b></div>
<div className="col-1 clan-list-item"><b>Level</b></div>
<div className="col-1 clan-list-item"><b>Trophies</b></div>
<div className="col-2 clan-list-item"><b>Role</b></div>
<div className="col-2 clan-list-item"><b>Last Seen</b></div>
<div className="col-1 clan-list-item"><b>Donated</b></div>
<div className="col-1 clan-list-item"><b>Donations Recieved</b></div>
</div>
 */
}
