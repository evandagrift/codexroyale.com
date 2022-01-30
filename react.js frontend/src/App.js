import logo from './logo.svg';
import './App.css';
import MyNavbar from './components/Navbar.js';
import RegisterPage from './Pages/RegisterPage';
import HomePage from './Pages/HomePage';
import {BrowserRouter, Route, Switch, useParams } from 'react-router-dom';
import React, { useContext, useState, useMemo } from 'react';
import { UserContext } from "./UserContext";
import UserSettingsPage from './Pages/UserSettingsPage copy';
import PlayerPage from './Pages/PlayerPage';
import LoginPage from './Pages/LoginPage';
import ClanPage from './Pages/ClanPage';
import ContactPage from './Pages/ContactPage';
import EmailVerificationPage from './Pages/EmailVerificationPage';
import ForgotPasswordPage from './Pages/ForgotPasswordPage';

function App() {
    const [user, setUser] = useState(null);

   if(localStorage.getItem('user') && user == null)
   {
    setUser(JSON.parse(localStorage.getItem('user')));
   }

  return (
    <div className="form-signin">
    <UserContext.Provider value={{user, setUser}}>
    <BrowserRouter>
          <MyNavbar  />
          
          <Route path="/" exact component={HomePage}  />

          <Route path="/login" exact  component={LoginPage}  />
          <Route path="/register" exact component={RegisterPage}  />
          <Route path="/settings"exact  component={UserSettingsPage}  />
          <Route path="/clan" exact component={ClanPage}  />
          <Route path="/contact" exact component={ContactPage}  />
          <Route path="/clan/:clanTag"> <ClanPage /> </Route>
          <Route path="/player"exact  component={PlayerPage}/>
          <Route path="/player/:playerTag"><PlayerPage /></Route>
          <Route path="/register/authenticate/:verificationCode"><EmailVerificationPage /></Route>
          <Route path="/forgotpassword/:passwordResetCode"exact  component={ForgotPasswordPage}/>
          <Route path="/forgotpassword"exact  component={ForgotPasswordPage}/>
        </BrowserRouter>
      </UserContext.Provider>
      </div>
      );
}

export default App;
