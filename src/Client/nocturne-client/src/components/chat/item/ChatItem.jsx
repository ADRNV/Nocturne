import React from "react";
import "./ChatItem.css";


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

                <select>
                    <option>Ban</option>
                    <option>Add to contacts</option>
                </select>
            
            </div>
        </div>
    )
}

export default ChatItem