import React from 'react'
import ChatItem from '../item/ChatItem'
import "./ChatList.css"

export default function ChatList({props, chats}) {
  return (
    <div className='ChatList'>
        {
            chats.map(chat => <ChatItem key={chat.name} chat={chat}/>)
        }
    </div>
  )
}
