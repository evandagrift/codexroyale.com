import logo from './logo.svg';
import './App.css';
import MyNavbar from './components/Navbar.js';
import HomePage from './Pages/HomePage';
import { Routes, Route, Navigate } from 'react-router-dom';
import React, { useContext, useState, useMemo } from 'react';
import { UserContext } from "./UserContext";
import PlayerPage from './Pages/PlayerPage';
import ClanPage from './Pages/ClanPage';
import ContactPage from './Pages/ContactPage';

function App() {
  const [user, setUser] = useState(null);

  if (localStorage.getItem('user') && user == null) {
    setUser(JSON.parse(localStorage.getItem('user')));
  }

  return (
    <div className="form-signin">
      <UserContext.Provider value={{ user, setUser }}>
        <MyNavbar />
        <Routes>

          {/* <Route path="/" exact component={TestPage}  /> */}
          <Route index element={<HomePage />} />
          <Route path="/" element={<HomePage />} />
          {/* <Route path="/login" exact component={LoginPage}  /> */}
          {/* <Route path="/register" exact component={RegisterPage}  /> */}
          {/* <Route path="/settings" exact component={UserSettingsPage}  /> */}
          <Route path="/clan" element={ClanPage} />
          <Route path="/contact" element={ContactPage} />
          <Route path="/clan/:clanTag" element={ClanPage} />
          <Route path="/player" element={PlayerPage} />
          <Route path="/player/:playerTag" element={PlayerPage} />
          {/* <Route path="/register/authenticate/:verificationCode"><EmailVerificationPage /></Route> */}
          {/* <Route path="/forgotpassword/:passwordResetCode"  component={ForgotPasswordPage}/> */}
          {/* <Route path="/forgotpassword" exact component={ForgotPasswordPage}/> */}
        </Routes>
      </UserContext.Provider>
    </div>
  );
}

export default App;
