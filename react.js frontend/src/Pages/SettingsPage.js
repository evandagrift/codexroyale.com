import React, { useState, useContext } from "react";
import { UserContext } from "../UserContext";
import PasswordInputFormControls from "../components/PasswordInputFormControls";
import { Redirect } from "react-router";

import { updateUserSettings } from "../Utilities/axios-functions";

const UserSettingsPage = () => {
  const { user, setUser } = useContext(UserContext);
  const [tag, setTag] = useState("");

  const [password, setPassword] = useState("");

  const [newPassword, setNewPassword] = useState("");
  const [newPassword2, setNewPassword2] = useState("");

  const [isValidPassword, setIsValidPassword] = useState(false);

  const updateUser = async (e) => {
    e.preventDefault();
    let tempUser = user;

    let passwordToSend = '';
    if(isValidPassword)
    {
      passwordToSend = newPassword;
      tempUser.password = password;
    }

    const newUser = await updateUserSettings(tempUser, tag, password, passwordToSend);
    
    setUser(newUser);
    if(localStorage.getItem("user")) { localStorage.setItem('user', JSON.stringify(newUser)); }

    //use user data to fill the settings
    //create component for the non password settings
    //create component for password reset have it drop down from a <a>
    //when click send push to backend
    //if no user reroute to login
  };

let passwordsNotMatch = '';

  if(password.length > 9 && newPassword.length > 9 && newPassword2.length > 9 && newPassword == newPassword2)
  {
    if(isValidPassword == false) setIsValidPassword(true);
  }
  else {
    if(newPassword.length > 0 && newPassword != newPassword2)
    {
passwordsNotMatch = (<p>passwords don't match</p>)
    }
  }
  
const handleNewPasswordChangeValue = e => setNewPassword(e.target.value);
const handleNewPassword2ChangeValue = e =>  setNewPassword2(e.target.value);

let settingsForm = '';
if(user)
{
settingsForm = (<form onSubmit={updateUser}>
  <p><b>Username:</b>{user.username}</p>
  <p><b>Email:</b>{user.email}</p>
  <p><b>Tag</b> <input
          type="tag"
          className="form-control"
          placeholder={user.tag}
          onChange={(e) => setTag(e.target.value)}
        /></p>
  <p><b>Clan Tag:</b>{user.clanTag}</p>
  <p><b>Password</b>  <input
          type="password"
          className="form-control"
          placeholder="Current Password"
          onChange={(e) => setPassword(e.target.value)}
        /></p>
        <h4>new password</h4>
<PasswordInputFormControls passwordChangeValue={handleNewPasswordChangeValue} password2ChangeValue={handleNewPassword2ChangeValue}/>
  <button type="submit">Update Settings</button>
  </form>);
}
else {settingsForm = (<Redirect to="/" />)}

  //username, and validated email remain static
  //
  return (
    <div>
      {settingsForm}
      {passwordsNotMatch}
    </div>
  );
};

export default UserSettingsPage;
