import { Container, CssBaseline, ThemeProvider, createTheme } from '@mui/material';
import '../../App.css';
import RoutingComponent from "../../RoutingComponent";
import Header from './Header';
import { useEffect, useState } from 'react';
import { useStoreContext } from '../context/StoreContext';
import agent from '../api/agent';
import LoadingComponent from './LoadingComponent';

export default function App() {
  const {cart, setCart} = useStoreContext();
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // const userId = getCookie('userId')
    // if (!userId ) {
    //   setLoading(false);
    //   return;
    // }
    console.log('Getting existing ShoppingCart');
    agent.ShoppingCart.list('3fa85f64-5717-4562-b3fc-2c963f66afa6')
      .then(cart => {
        console.log(cart);
        setCart(cart)})
      .catch(error => console.log(error))
      .finally(() => setLoading(false));
  }, [setCart]);

  const [darkMode, setDarkMode] = useState(false);
  const paletteType = darkMode ? 'dark' : 'light';
  const theme = createTheme({
    palette:{
      mode: paletteType,
      background:{
        default: paletteType === 'light' ? '#eaeaea' : '#121212'
      }
    }
  });

  function handleThemeChange(){
    setDarkMode(!darkMode);
  }

  if(loading) return <LoadingComponent message='Initialising app ...'/>

  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <Header darkMode={darkMode} handleThemeChange={handleThemeChange}/>
      <Container>
        <RoutingComponent />
      </Container>
    </ThemeProvider>
  );
}