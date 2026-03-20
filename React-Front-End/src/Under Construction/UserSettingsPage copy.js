import React, {useState, useContext} from "react";
import {UserContext} from "../UserContext";
import { axios } from "../axios";
import { Link } from "react-router-dom";

const UserSettingsPage = () => {    
    const {user, setUser} = useContext(UserContext);
    const[password, setPassword] = useState('');
    const[email,setEmail] = useState('');
    const[role,setRole] = useState('');
    const[tag,setTag] = useState('');
    const[clanTag,setClanTag] = useState('');

    //check legitimacy of either user or clan Tag
    const UpdateUser = (e) => {
        e.preventDefault();
        const config = { headers: { Authorization: `bearer ${user['token']}`}};
        
        //axios headers
        //bearer assign
        //put
      

      
      axios.put('Users', {
        Username: user['username'],
        Password: user['password'],
        Token: user['token'],
        Email: email,
        Tag: tag,
        ClanTag: clanTag,
        Role: role
      },config)
      .then((response) => {
      }, (error) => {
        console.log(error);
      });

        //make put request w/ Bearer Token
    }

//make switch based of role level
//end users can't change
//Username
//role
//token
    
    return (
        <div>
            <form onSubmit={UpdateUser}>

                <h1 className="h3 mb-3 fw-normal">Change your profile settings: <b>{user['username']}</b> </h1>

                <label>Email
                <input type="email" id="inputEmail" className="form-control" placeholder={user['email']} onChange={e => setEmail(e.target.value)}required/>
                </label>
                
                <label>Tag
                <input type="tag" id="inputTag" className="form-control" placeholder={user['tag']} onChange={e => setTag(e.target.value)}required />
                </label>

                <label>ClanTag
                <input type="clantag" id="inputClanTag" className="form-control" placeholder={user['clanTag']} onChange={e => setClanTag(e.target.value)}required />
                </label>

                <label>Role
                <input type="role" id="inputRole" className="form-control" placeholder={user['role']} onChange={e => setRole(e.target.value)}required />
                </label>
                <br/>
                
                <Link className="link" to="/logintest">change password</Link>

                <br/>

                <button className="w-100 m-2 btn btn-lg btn-primary" type="submit">Update User</button>

            </form>
        </div>
    );
};

export default UserSettingsPage;