import  React from 'react';
import Tshirt from '../../best-t-shirts-for-men.png'
import Sys32 from '../../sys32.png'

export default function MainPage() {
    return(

        <div style={{ textAlign: 'center', padding: '50px' }}>
            <h1 style={{ fontSize: '3em' }}>Welcome to BuzzHouse</h1>
            <img src={Tshirt} alt="BuzzHouse" style={{ width: '125%', height: 'auto', margin: '20px 0' }} />
            <p style={{ fontSize: '1.5em' }}>Our products will give the buzz you lack.</p>
            <img src={Sys32} alt="BuzzHouse" style={{ width: '205%', height: 'auto', margin: '20px 0' }} />
            <p style={{ fontSize: '1.5em' }} >We do not hold responsibilities for the action of our designs</p>
        </div>

    )
}