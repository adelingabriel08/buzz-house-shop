import { Route, Routes } from 'react-router-dom';
import OrderHistory from './pages/OrderHistory/OrderHistory';
import CartPage from './pages/Cart/CartPage';
import CustomizePage from './pages/CustomizeItem/CustomizePage';
import HomePage from './pages/Home/HomePage';
import ItemPage from './pages/Item/ItemPage';
import ItemsPage from './pages/Item/ItemsPage';

export default function RoutingComponent() {
    return (
        <Routes>
            <Route path="/" Component={HomePage} />
            <Route path="/cart" Component={CartPage} />
            <Route path="/customize" Component={CustomizePage} />
            <Route path="/item/:id" Component={ItemPage} />
            <Route path="/items" Component={ItemsPage} />
            <Route path="/order-history" Component={OrderHistory} />
        </Routes>
    );
}
