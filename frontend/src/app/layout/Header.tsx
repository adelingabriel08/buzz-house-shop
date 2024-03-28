import { AppBar, Toolbar, Typography } from "@mui/material";
import GoogleLogin from "../../Components/GoogleLogin";
import GoogleLogout from "../../Components/GoogleLogout";
import {useEffect} from "react";
import {gapi} from 'gapi-script'

const clientId = "703288565306-jt1s2dbhmgku13b75vnulhap1pnrn7pu.apps.googleusercontent.com";


export default function Header(){

    useEffect(() =>{

        function start (){
            gapi.client.init({
                clientId : clientId,
                scope: ""
            })
        };
        gapi.load('client:auth2',start)

    });

    return(
        <AppBar position="static" sx={{ marginBottom: 4 }}>
            <Toolbar>
                <Typography variant="h6" sx={{ flexGrow: 1 }}>
                    Buzz-House-SHOP
                </Typography>
                     <GoogleLogin/>
                    <GoogleLogout/>
            </Toolbar>
        </AppBar>
    )
}