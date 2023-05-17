import React from "react";
import "./ChatItem.css";
import OptionsSelector from "../../select/OptionsSelector";


const ChatItem = (props) => {

    return (
        <div className="ChatItem">  
               <div className="info">
            <div className="imagePreview">
                <img alt="image"></img>
            </div>

            <div className="mainInfo">

                <p className="header">{props.chat.name}</p>
            
                <div className="message">
                    <p>{props.chat.message}</p>
                </div>
                </div>
                
            </div>

            <div className="options">

                <OptionsSelector options={[
                    {name:"Ban", value:"stub"},
                    {name:"Unban", value:"stub"},
                    {name:"Add to contacts", value:"stub"}
                ]}/>
            
            </div>
        </div>
    )
}

export default ChatItem