import React, { useState, useContext } from "react";
import { Redirect, useParams } from 'react-router-dom';

import { UserContext } from "../UserContext";
import { axios } from "../axios";

const EmailVerificationPage = () => {


    const { user, setUser } = useContext(UserContext);
    
    const { verificationCode } = useParams();

    const [verificationSent,setVerificationSent] = useState(false)

    const authenticate = async (e) => {
        let codeToSend = verificationCode;
        await axios.post("Users/VerifyAccount/"+ codeToSend
        )
        .then(
          (response) => {
            setUser(response.data);
          },
          (error) => {
            console.log(error);
          });
    

        }

        let draw = 'verifying account';

        if(!verificationSent) {
            authenticate();
            setVerificationSent(true);
        }
        else 
        {
          draw = (<Redirect to="/" />)
        }
        
    return (<div>
        {draw}
        </div>
    );
    
    
}

export default EmailVerificationPage;


//ask someone why this doesn't work
   // draw = (user) ? (<div><h2>Verifying Email</h2></div>):(<div><Redirect to="/" /></div>) ;
