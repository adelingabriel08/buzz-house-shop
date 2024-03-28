import {GoogleLogin} from 'react-google-login'

const clientId = "703288565306-jt1s2dbhmgku13b75vnulhap1pnrn7pu.apps.googleusercontent.com";

function Login () {

    // @ts-ignore
    const onSuccess = (res) => {
        console.log ("On success",res.profileObj);
    }

    // @ts-ignore
    const onFailure = (res) => {
        console.log ("On Failure", res);
    }

    return (

        <div>
            <GoogleLogin
                clientId={clientId}
                buttonText="Login"
                onSuccess={onSuccess}
                onFailure={onFailure}
                isSignedIn={true}
            />


        </div>
    );

}

export default Login;