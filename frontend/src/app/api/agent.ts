import axios, { AxiosResponse } from "axios";
import { Product } from "../models/product";
import {getIdToken} from "../util/util";
import { Cart } from "../models/cart";
import { CartItem } from "../models/cartItem";
import { Order as OrderModel } from "../models/order";

axios.defaults.baseURL = 'http://localhost:5147/api/';

const responseBody = (response: AxiosResponse) => response.data;

const requests = {
    get: (url: string) => axios.get(url, {headers:{...getAuthorizationHeader()}}).then(responseBody),
    post: (url: string, body: {}) => axios.post(url, body, {headers:{...getAuthorizationHeader()}}).then(responseBody),
    put: (url: string, body: {}) => axios.put(url, body, {headers:{...getAuthorizationHeader()}}).then(responseBody),
    delete: (url: string) => axios.delete(url, {headers:{...getAuthorizationHeader()}}).then(responseBody),
}

const Catalog = {
    list: () => requests.get('products'),
    create: (product: Product) => requests.post(`products`, {product}),
    details: (productId: string) => requests.get(`products/${productId}?productId=${productId}`),
}

const Order = {
    list: () => requests.get('orders'),
    details: (orderId: string) => requests.get(`orders/orderId/${orderId}?orderId=${orderId}`),
    update: (order: OrderModel) => requests.put(`orders/${order.id}?orderId=${order.id}`, {order}),
    delete : (id:number) => requests.delete(`orders/${id}`)
}

const ShoppingCart = {
    list: () => requests.get(`shoppingcart`),
    create: (cart: Cart) => requests.post(`shoppingcart?`, {cart}),
    addItem: (cartId: string, cartItem: CartItem) => requests.post(`shoppingcart/${cartId}/items?shoppingCartId=${cartId}`, cartItem),
    updateCartitem: (cartId: string, cartItem: CartItem) => requests.put(`shoppingcart/${cartId}/items?shoppingCartId=${cartId}`, cartItem),
    removeItem: (cartId: string, productId: string) => requests.delete(`shoppingcart/${cartId}/items/${productId}?shoppingCartId=${cartId}&productId=${productId}`)
}

const agent = {
    Catalog,
    Order,
    ShoppingCart
}


const getAuthorizationHeader = () => {
    return {
        "Authorization" : "Bearer " + getIdToken()
    };
}

export default agent; 