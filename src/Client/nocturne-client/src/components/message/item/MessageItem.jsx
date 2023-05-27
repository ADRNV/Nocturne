import React from 'react'
import './MessageItem.css'

export default function MessageItem({props, message}) {
  return (
    <div class="message-blue" props={props}>
        <p class="message-content">{message.content}</p>
        <div class="message-timestamp-left">{message.date}</div>
    </div>
  )
}
