import React from 'react'
import OptionsSelector from '../../select/OptionsSelector'
import ImageButton from '../../button/imageButton/ImageButton'
import MessageItem from '../item/MessageItem'
import './MessagesList.css'

export default function MessagesList({props, messages, user}) {
  
    const options = [
        {name:"Ban", value:"ban"},
        {name:"Add to contacts", value:"add"}
    ]
  
    return (
    <div className='MessagesList'>
        <div className='container'>
            <div className='messageBar'>
                <div className='info'>
                    <ImageButton icon={user.image}/>
                    <p>{user.name}</p>
                </div>  

                <div className='optionsContainer'>
                    {
                        user === undefined ? 
                        <p></p> :
                        <OptionsSelector className="optionsSelector" options={options}/>   
                    }
                </div>
            </div>

            
            <div className='list'>
                {
                    messages !== undefined ?  
                    messages.map(message => <MessageItem key={messages.id} message={message}/>):
                    <p>No selected chat</p>
                }
            </div>
        </div>
    </div>
  )
}
