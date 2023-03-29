import React from 'react';
import "./PrimaryButton.css"

const PrimaryButton = ({props, children }) => {
  return (
    <button {...props} className='PrimaryButton'>{children}</button>
  )
}

export default PrimaryButton