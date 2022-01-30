import React, {useState, useContext} from "react";
import {Redirect} from 'react-router-dom';

import { UserContext } from "../UserContext";
import { LoginFunctionAsync } from "../Utilities/axios-functions";

const LoginPage = () => {

    //useState returns an array, we destructure it into a variable and function
    const [name, setName] = useState('');
    const [password, setPassword] = useState('');
    const {user, setUser, player, setPlayer} = useContext(UserContext);


    let login = '';

        const makePostRequest = async (e) => {
            e.preventDefault();
        let stayLogged = false;
        if(document.getElementById('stayLoggedIn').checked) {stayLogged= true; }
        
        let tempUser = await LoginFunctionAsync(name,password);
        setUser(tempUser);


        if(stayLogged&&tempUser!=null) { localStorage.setItem('user', JSON.stringify(tempUser)); }
        
    }


    if(user == null)
    {
        login = (
        <form className="d-inline-block" onSubmit = {makePostRequest}>
            <h1 className="h3 mb-3">Clash Codex Login</h1>

            <input type="username" id="inputUsername" className="form-control" placeholder="Username" onChange={ e => setName(e.target.value)} required />

            <input type="password" id="inputPassword" className="form-control" placeholder="Password" onChange={e => setPassword(e.target.value)} required />
           <div> <input id="stayLoggedIn" type="checkbox" ></input> stay logged in</div>

            <button className="w-100 btn btn-lg btn-primary m-2" type="submit">Sign in</button>
            <p><a href="/forgotpassword">i forgot my password</a></p>
        </form>);

    }
    else { login =(<Redirect to="/" />);}

    return (
        <div>
            {login}
        </div>
    );
};

export default LoginPage;