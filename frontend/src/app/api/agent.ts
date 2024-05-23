import axios, { AxiosResponse } from "axios";
import { Product } from "../models/product";
import { Cart, CartItem } from "../models/cart";
import {getIdToken} from "../util/util";

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
    create: (product: Product) => requests.get(`products?product=${product}`),
    details: (id: string) => requests.get(`products/${id}`),
    update: (id:number) => requests.put(`products/${id}`, {}),
    delete : (id:number) => requests.delete(`products/${id}`)
}

const Orders = {
    list: () => requests.get('orders'),
    list_date: () => requests.get('orders/createdDate'),
    order_status: (orderStatus : string) => requests.get(`orders/orderStatus/${orderStatus}`),
    detail: (id: number) => requests.get(`orders/${id}`),
    update: (id: number) => requests.put(`orders/${id}`, {}),
    delete : (id:number) => requests.delete(`orders/${id}`)
}

const ShoppingCart = {
    get: (cartId: string) => requests.get(`shoppingcart/${cartId}?shoppingCartId=${cartId}`),
    create: (cart: Cart) => requests.post(`shoppingcart?shoppingCart=${cart}`, {}),
    addItem: (userId: string, cartItem: CartItem) => requests.post(`shoppingcart/${userId}/items?shoppingCartId=${userId}`, cartItem),
    removeItem: (productId: string, quantity = 1) => requests.delete(`shoppingcart?productId=${productId}&quantity=${quantity}`)
}

const agent = {
    Catalog,
    Orders,
    ShoppingCart
}


const getAuthorizationHeader = () => {
    return {
        "Authorization" : "Bearer " + getIdToken()
    };
}

export default agent; 