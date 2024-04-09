import { AppBar, Box, Switch, Toolbar, Typography } from "@mui/material";
import GoogleLogin from "../../Components/GoogleLogin";
import GoogleLogout from "../../Components/GoogleLogout";
import {useEffect} from "react";
import {gapi} from 'gapi-script'

const clientId = "703288565306-jt1s2dbhmgku13b75vnulhap1pnrn7pu.apps.googleusercontent.com";
interface Props{
    darkMode: boolean;
    handleThemeChange: () => void;
}

export default function Header({darkMode, handleThemeChange}:Props){

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
        <AppBar position="static" sx={{ mb: 4 }}>
            <Toolbar>
                <Box sx={{ 
                    display: 'flex', 
                    flexGrow: 1,
                    alignItems: 'center' }}>
                    <Typography variant="h6">
                        Buzz-House-SHOP
                    </Typography>
                    <Switch checked={darkMode} onChange={handleThemeChange}/>
                </Box>
                <GoogleLogin/>
                <GoogleLogout/>
            </Toolbar>
        </AppBar>
    )
}