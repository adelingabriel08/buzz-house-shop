import React, { useState } from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import OrderHistory from '../src/pages/OrderHistory/OrderHistory';
import CartPage from '../src/pages/Cart/CartPage';
import CustomizePage from '../src/pages/CustomizeItem/CustomizePage';
import HomePage from '../src/pages/Home/HomePage';
import ItemPage from '../src/pages/Item/ItemPage';
import ItemsPage from '../src/pages/Item/ItemsPage';
import Login from '../src/pages/Login/LoginPage';
import { Product } from './app/models/product';
function RoutingComponent() {
    //For testing purposes only
    const[products, setProducts] = useState<Product[]>([
        {
            id: 1,
            name: "product",
            description: "description product",
            price: 1000,
            pictureUrl: "/images/testImage.png",
            brand: "brand product"
        }
    ])
    return (
        <Router>
            <Routes>
            <Route path="" element={<HomePage products={products} />} />
            <Route path="/cart" Component={CartPage} />
            <Route path="/customize" Component={CustomizePage} />
            <Route path="/item" Component={ItemPage} />
            <Route path="/items" Component={ItemsPage} />
            <Route path="/order-history" Component={OrderHistory} />
            <Route path="/login" Component={Login} />
            </Routes>
        </Router>
    );
}

export default RoutingComponent;