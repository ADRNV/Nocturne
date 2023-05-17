import React from 'react'
import ChatItem from '../item/ChatItem'
import "./ChatList.css"

export default function ChatList(props) {
  return (
    <div className='ChatList'>
        {
            props.chats.map(chat => <ChatItem key={chat.name} chat={chat}/>)
        }
    </div>
  )
}
