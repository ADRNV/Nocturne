import React from 'react'
import './MessageArea.css'

export default function MessageInput(props) {
  return (
    <textarea {...props} className='MessageArea' type='text'/>
  )
}
