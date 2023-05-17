import React from 'react'
import "./OptionsSelector.css"

export default function OptionsSelector({onChange, options}) {
  return (
    <div className='OptionsSelector'>

        <select className='optionsList' onChange={event => onChange(event.target.value)}>
        
            {options.map(option => {
                return <option className='optionItem' key={option.name} value={option.value}>
                    {option.name}
                        </option>
            })
        }
        </select>
    </div>
  )
}
