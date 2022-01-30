import React, { useState, useContext } from "react";
import { Redirect } from "react-router-dom";

import { UserContext } from "../UserContext";
import { axios } from "../axios";
import PasswordInputFormControls from "../components/PasswordInputFormControls"

const RegisterPage = () => {
  //useState returns an array, we destructure it into a variable and function
  const [userName, setUserName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [password2, setPassword2] = useState("");
  const [tag, setTag] = useState("");

  const [registered, setRegistered] = useState(false);
  const [invalidCredentials, setInvalidCredentials] = useState();

  const { user, setUser } = useContext(UserContext);

  const makePostRequest = (e) => {

      axios
        .post("Users/signup", {
          Username: userName,
          Password: password,
          Email: email,
          Tag: tag,
        })
        .then(
          (response) => {
            if (response.status == false) {
                setInvalidCredentials(true);
            } else {
                setRegistered(true);
            }
          },
          (error) => {
            console.log(error);
          }
        );
    
  };

  const checkFields = () => {
    return true;
  };
  let usernameEmailElements = (
    <div>
      <span>
        <b>Username</b>
      </span>
      <input
        type="username"
        id="inputUsername"
        className="form-control"
        placeholder="Username"
        onChange={(e) => setUserName(e.target.value)}
        required
      />

      <span>
        <b>Email</b>
      </span>
      <input
        type="email"
        id="inputEmail"
        className="form-control"
        placeholder="Email"
        onChange={(e) => setEmail(e.target.value)}
        required
      />
    </div>
  );

  let userTagElement = (
    <div>
      <span>
        <b>Clash Royale Tag</b>
      </span>
      <input
        id="inputTag"
        className="form-control"
        placeholder="#3x4mp13"
        onChange={(e) => setTag(e.target.value)}
        required
      />
    </div>
  );

  let loginButton = (
    <button className="w-100 btn btn-lg btn-primary m-1" onClick={makePostRequest}>
      Register
    </button>
  );  
  let resendVerificationLink = (
    <button className="w-60 btn btn-sm btn-primary m-1" onClick={makePostRequest}>
     Resend Verification Link
    </button>
  );


  let invalidCredentialsElement = ''
  if(invalidCredentials){
    invalidCredentialsElement = (<p className="warning-text">Username Or Email is already in use</p>);
  }
  else invalidCredentialsElement = '';

let draw = '';


const handlePasswordChangeValue = e => setPassword(e.target.value);
const handlePassword2ChangeValue = e =>  setPassword2(e.target.value);

if(registered == false)
{
draw = (<div><h1 className="h3 mb-3 fw-normal">Please Register</h1>
{usernameEmailElements}
<PasswordInputFormControls passwordChangeValue={handlePasswordChangeValue} password2ChangeValue={handlePassword2ChangeValue}/>
{userTagElement}
{loginButton}</div>);
}
else draw = (<div>
  <p>Check your email for your verifcation link</p>
<div>{resendVerificationLink}</div>
</div>);

  return (
    <div>
      {draw}
    </div>
  );
};

export default RegisterPage;
