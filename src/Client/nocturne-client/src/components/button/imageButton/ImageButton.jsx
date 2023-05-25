import React from 'react'
import './ImageButton.css'

export default function ImageButton({props, icon, onClick}) {
  return (
    <div className='ImageWithButton'>
        <img className='icon' src={icon} onClick={onClick}/>
    </div>
  )
}
