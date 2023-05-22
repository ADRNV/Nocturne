import React from 'react'
import StringInput from '../../input/StringInput'
import PrimaryButton from '../../button/PrimaryButton'
import './SigninForm.css'

export default function SignInForm() {
  return (
    <div className='SignInForm'>
        
        <div className='container'>
         <p>Login</p>
            <StringInput/>
            <p>Password</p>
            <StringInput type="password"/>
            <PrimaryButton>Sign in</PrimaryButton>
        </div>
        
    </div>
  )
}
