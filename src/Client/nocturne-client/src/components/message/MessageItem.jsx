import React from 'react'
import './MessageItem.css'

export default function MessageItem(props) {
  return (
    <div className="container">
      <div className="message-blue">
        <p className='message-from'>{props.message.from}</p>
        <p className="message-content">{props.message.content}</p>
        <div className="message-timestamp-left">{props.message.dateTime}</div>
      </div>
    </div>
  )
}
