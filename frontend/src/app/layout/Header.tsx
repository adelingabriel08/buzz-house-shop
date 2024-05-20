import { AppBar, Box, List, ListItem, Switch, Toolbar, Typography } from "@mui/material";
import GoogleLogin from "../../Components/GoogleLogin";
import GoogleLogout from "../../Components/GoogleLogout";
import {useEffect} from "react";
import {gapi} from 'gapi-script';
import { NavLink } from "react-router-dom";

interface Props{
    darkMode: boolean;
    handleThemeChange: () => void;
}
const clientId = "703288565306-jt1s2dbhmgku13b75vnulhap1pnrn7pu.apps.googleusercontent.com";
const midLinks = [
    {title: 'cart', path: '/cart'},
    {title: 'customize', path: '/customize'},
    {title: 'item', path: '/item'},
    {title: 'items', path: '/items'},
    {title: 'order-history', path: '/order-history'},
]

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
                    <List>
                        {midLinks.map(({title, path}) => (
                            <ListItem 
                                component={NavLink}
                                to={path}
                                key={path}
                                sx={{color: 'inherit', typography: 'h6'}}
                            >
                                {title.toUpperCase()}
                            </ListItem>
                        ))}
                    </List>
                </Box>
                <GoogleLogin/>
                <GoogleLogout/>
            </Toolbar>
        </AppBar>
    )
}