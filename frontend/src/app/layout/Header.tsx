import { AppBar, Badge, Box, IconButton, List, ListItem, Switch, Toolbar, Typography } from "@mui/material";
import GoogleLogin from "../../Components/GoogleLogin";
import GoogleLogout from "../../Components/GoogleLogout";
import {useEffect} from "react";
import {gapi} from 'gapi-script';
import { Link, NavLink } from "react-router-dom";
import { ShoppingCart } from "@mui/icons-material";
import { useStoreContext } from "../context/StoreContext";

interface Props{
    darkMode: boolean;
    handleThemeChange: () => void;
}
const clientId = "703288565306-jt1s2dbhmgku13b75vnulhap1pnrn7pu.apps.googleusercontent.com";
const midLinks = [
    {title: 'Our products', path: '/products'},
    {title: 'order-history', path: '/order-history'},
]
const navStyles = {
    color: 'inherit', 
    textDecoration: 'none',
    typography: 'h6', 
    width: 'auto',
    whiteSpace: 'nowrap',
    overflow: 'hidden',
    '&:hover': {
        color: 'grey.500',
        textShadow: '2px 2px 4px rgba(0, 0, 0, 0.5)'
    },
    '&.active':{
        color: 'text.secondary'
    }
}

export default function Header({darkMode, handleThemeChange}:Props){
    const {cart} = useStoreContext();
    const itemCount = cart?.cartItems?.reduce((sum, item) => sum + item.quantity, 0);

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
            <Toolbar sx={{
                display: 'flex',
                justifyContent: 'space-between',
                alignItems: 'center'
            }}>
                <Box display='flex' alignContent='center'>
                    <Typography variant="h6"
                                component={NavLink}
                                to={'/'}
                                key={'/'}
                                sx={navStyles}>
                        BUZZ-HOUSE Shop
                    </Typography>
                    <Switch checked={darkMode} onChange={handleThemeChange}/>
                </Box>
                    
                <List sx={{display: 'flex'}}>
                    {midLinks.map(({title, path}) => (
                        <ListItem 
                            component={NavLink}
                            to={path}
                            key={path}
                            sx={navStyles}>
                                {title.toUpperCase()}
                        </ListItem>
                    ))}
                </List>
                    
                <Box display='flex' alignContent='center'>
                    <IconButton component={Link} to='/cart' size="large" 
                                sx={navStyles}>
                        <Badge badgeContent={itemCount} color="secondary">
                            <ShoppingCart />
                        </Badge>
                    </IconButton>
                    <GoogleLogin/>
                    <GoogleLogout/>
                </Box>    
            </Toolbar>
        </AppBar>
    )
}