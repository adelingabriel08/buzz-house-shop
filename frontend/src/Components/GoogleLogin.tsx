import {GoogleLogin} from 'react-google-login'
import {persistIdToken} from  "../app/util/util"

const clientId = "703288565306-jt1s2dbhmgku13b75vnulhap1pnrn7pu.apps.googleusercontent.com";

function Login () {

    // @ts-ignore
    const onSuccess = (res) => {
        persistIdToken(res.tokenId);
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