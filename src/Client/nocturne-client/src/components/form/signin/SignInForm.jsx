import React from 'react'
import StringInput from '../../input/StringInput'
import PrimaryButton from '../../button/PrimaryButton'
import './SigninForm.css'
import SignWithButton from '../../signInButton/button/SignWithButton'
import googleIcon from './googleIcon.png'
import microsoftIcon from './microsoftIcon.png'

export default function SignInForm() {
  return (
    <div className='SignInForm'>
        
        <div className='container'>
         <p>Login</p>
            <StringInput/>
            <p>Password</p>
            <StringInput type="password"/>
            <PrimaryButton>Sign in</PrimaryButton>
            
            <div className='signWithContainer'>
              <SignWithButton icon={googleIcon} onClick={() => alert()}/>
              <SignWithButton icon={microsoftIcon} onClick={() => alert()}/>
            </div>
            
        </div>
        
    </div>
  )
}
