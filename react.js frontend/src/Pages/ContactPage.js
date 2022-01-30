import React from "react";

const ContactPage = () => {


    let draw =(<div>
        <h1>Contact</h1>
        <p>Hi, we'd love to hear from you. If you find a bug or have a feature you would to see implemented you can email me at <a href="mailto:clashroyaletracker@gmail.com">clashroyaletracker@gmail.com</a></p>
        </div>);


    
    return (
        <div className="p-1">
            {draw}
        </div>
    );
};

export default ContactPage;