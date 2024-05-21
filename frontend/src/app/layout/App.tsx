import { Container, CssBaseline, ThemeProvider, createTheme } from '@mui/material';
import '../../App.css';
import RoutingComponent from "../../RoutingComponent";
import Header from './Header';
import { useEffect, useState } from 'react';
import { useStoreContext } from '../context/StoreContext';
import { getCookie } from '../util/util';
import agent from '../api/agent';
import { error } from 'console';
import LoadingComponent from './LoadingComponent';

export default function App() {
  const {setCart} = useStoreContext();
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const userId = getCookie('userId')
    // if(userId){
    //   agent.Cart.get()
    //     .then(cart => setCart(cart))
    //     .catch(error => console.log(error))
    //     .finally(() => setLoading(false))
    // }
    setCart({
      id: 1,
      userId: 'hackerId',
      items: [{
          productId: 1,
          name: "product1",
          price: 1000,
          pictureUrl: "http://picsum.photos/100",
          quantity: 1
      },
      {
          productId: 2,
          name: "product2",
          price: 2000,
          pictureUrl: "http://picsum.photos/200",
          quantity: 2
      },
      {
          productId: 3,
          name: "product3",
          price: 3000,
          pictureUrl: "http://picsum.photos/300",
          quantity: 3
      }]
    })
    setLoading(false);
  }, [setCart])

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