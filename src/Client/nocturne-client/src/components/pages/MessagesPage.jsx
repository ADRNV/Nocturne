import React from 'react'
import ChatList from '../chat/list/ChatList'
import MessagesList from '../message/list/MessageList'
import './MessagesPage.css'

export default function MessagesPage({props, chats, user}) {
  return (
    <div className='MessagesPage'>
        <ChatList className="chatList" chats={chats}/>
        <MessagesList className="messagesList" user={user}/>
    </div>
  )
}
