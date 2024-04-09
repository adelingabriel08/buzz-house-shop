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
    return (
        <Router>
            <Routes>
                <Route path="" Component={HomePage} />
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