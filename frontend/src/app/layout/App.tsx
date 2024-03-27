import { Container } from '@mui/material';
import '../../App.css';
import RoutingComponent from "../../Routing_Component";
import Header from './Header';
export default function App() {
  return (
    <>
      <Header />
      <Container>
        <RoutingComponent />
      </Container>
    </>
  );
}