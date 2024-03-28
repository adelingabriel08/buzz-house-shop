import React, { useState } from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import OrderHistory from '../src/pages/OrderHistory/OrderHistory';
import CartPage from '../src/pages/Cart/CartPage';
import CustomizePage from '../src/pages/CustomizeItem/CustomizePage';
import HomePage from '../src/pages/Home/HomePage';
import ItemPage from '../src/pages/Item/ItemPage';
import ItemsPage from '../src/pages/Item/ItemsPage';
import { Product } from './app/models/product';
function RoutingComponent() {
    //For testing purposes only
    const[products, setProducts] = useState<Product[]>([
        {
            id: 1,
            name: "product1",
            description: "description product1",
            price: 1000,
            pictureUrl: "http://picsum.photos/100",
            brand: "brand product1"
        },
        {
            id: 2,
            name: "product2",
            description: "description product2",
            price: 2000,
            pictureUrl: "http://picsum.photos/200",
            brand: "brand product2"
        },
        {
            id: 3,
            name: "product3",
            description: "description product3",
            price: 3000,
            pictureUrl: "http://picsum.photos/300",
            brand: "brand product3"
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
            </Routes>
        </Router>
    );
}

export default RoutingComponent;