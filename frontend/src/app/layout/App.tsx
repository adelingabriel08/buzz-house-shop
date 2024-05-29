import { Container, CssBaseline, ThemeProvider, createTheme } from '@mui/material';
import '../../App.css';
import RoutingComponent from "../../RoutingComponent";
import Header from './Header';
import { useState } from 'react';

export default function App() {
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

  // if(loading) return <LoadingComponent message='Initialising app ...'/>

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