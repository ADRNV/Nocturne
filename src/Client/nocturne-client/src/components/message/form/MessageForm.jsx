import React from 'react'
import PrimaryButton from '../../button/PrimaryButton'
import MessageArea from '../textarea/MessageArea'
import './MessageForm.css'

export default function MessageForm() {
  return (
    <div className='MessageContainer'>
        <MessageArea className="messageArea"></MessageArea>
        <PrimaryButton className="primaryButton" >Send</PrimaryButton>
    </div>
  )
}
