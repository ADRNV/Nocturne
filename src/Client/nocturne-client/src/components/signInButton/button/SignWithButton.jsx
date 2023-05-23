import React from 'react'
import './SignWithButton.css'

export default function SignWithButton({props, icon, onClick}) {
  return (
    <div className='SignWithButton'>
        <img className='icon' src={icon} onClick={onClick}/>
    </div>
  )
}
