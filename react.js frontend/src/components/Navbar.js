import React, {useState, useContext} from "react";
import { Link } from "react-router-dom";
import { UserContext } from "../UserContext";

import { Navbar,Nav,NavDropdown,Form,FormControl,Button } from 'react-bootstrap'; 

const MyNavbar = () => {

const { user, setUser} = useContext(UserContext);

let navBar = '';
if(user != null)
{
  navBar = (
    <ul class="navbar-nav">
        <li className="nav-item">
          <Link className="nav-link" to="/clan">Clan</Link>
        </li>
        <li className="nav-item">
          <Link className="nav-link" to="/player">Player</Link>
        </li>
        <li className="nav-item">
          <Link className="nav-link" to="/settings">User Settings</Link>
        </li>
      </ul>
  )
}
else{
navBar = (
  <ul class="navbar-nav">
        <li className="nav-item">
          <Link className="nav-link" to="/login">login</Link>
        </li>
        <li className="nav-item">
          <Link className="nav-link" to="/register">signup</Link>
        </li>
      </ul>
)
}










let loggedOutNav = (<div>
  <Navbar bg="dark" variant="dark" expand="lg" sticky="top" className="p-2">
  <Link to="/" className="navbar-brand">Codex Royale</Link>
  <Navbar.Toggle aria-controls="basic-navbar-nav" />
<Navbar.Collapse id="basic-navbar-nav"className="justify-content-end">
<Nav className="mr-auto">
<Link to="/Login" className="nav-link">Login</Link>
<Link to="/Register" className="nav-link">Register</Link>
<Link to="/Contact" className="nav-link">Contact</Link>
</Nav></Navbar.Collapse>
</Navbar>
</div>);

let currentNav = '';

if(user != null){
  
    currentNav = (<div>
      <Navbar bg="dark" variant="dark" expand="lg" sticky="top" className="p-2">
      <Link to="/" className="navbar-brand">Codex Royale</Link>
      <Navbar.Toggle aria-controls="basic-navbar-nav" />
      <Navbar.Collapse id="basic-navbar-nav"className="justify-content-start">
    <Nav className="mr-auto">
    <Link to="/Player" className="nav-link">Player</Link>
    <Link to="/Clan" className="nav-link">Clan</Link>
    </Nav></Navbar.Collapse>
  <Navbar.Collapse id="basic-navbar-nav"className="justify-content-end">
    <Nav className="mr-auto">
    <Link to="/Settings" className="nav-link">Settings</Link>
    <Link to="/Contact" className="nav-link">Contact</Link>
    </Nav></Navbar.Collapse>
    </Navbar>
    </div>);
}
else{
  currentNav = loggedOutNav;

}


return(<div>
{currentNav}
</div>
  
);

};

export default MyNavbar;