import { Route, Routes } from 'react-router-dom';
import OrderHistory from './pages/OrderHistory/OrderHistoryPage';
import CartPage from './pages/Cart/CartPage';
import CustomizePage from './pages/CustomizeItem/CustomizePage';
import ProductPage from './pages/Home/ProductPage';
import MainPage from './pages/Main/MainPage';
import ItemPage from './pages/Item/ItemPage';
import CheckoutPage from './pages/Checkout/CheckoutPage';
import OrderDetails from './pages/OrderHistory/OrderDetailsPage';

export default function RoutingComponent() {
    return (
        <Routes>
            <Route path="/products" Component={ProductPage} />
            <Route path="/" Component = {MainPage}/>
            <Route path="/cart" Component={CartPage} />
            <Route path="/checkout" Component={CheckoutPage} />
            <Route path="/customize" Component={CustomizePage} />
            <Route path="/item/:id" Component={ItemPage} />
            <Route path="/order-history" Component={OrderHistory} />
            <Route path="/order-history/order/:id" Component={OrderDetails} />
        </Routes>
    );
}
