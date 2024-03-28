import {GoogleLogout} from "react-google-login";

const clientId = "703288565306-jt1s2dbhmgku13b75vnulhap1pnrn7pu.apps.googleusercontent.com";

function Logout () {

    // @ts-ignore
    const onSuccess = () => {
        console.log ("Log Out ok");
    }

    return(
        <div >
            <GoogleLogout
        clientId={clientId}
        buttonText="Logout"
        onLogoutSuccess={onSuccess}
            />


        </div>

    );
}

export default Logout;