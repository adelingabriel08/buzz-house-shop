import axios, { AxiosResponse } from "axios";
import { Product } from "../models/product";
import { Cart } from "../models/cart";

axios.defaults.baseURL = 'http://localhost:5147/api/';

const responseBody = (response: AxiosResponse) => response.data;

const requests = {
    get: (url: string) => axios.get(url).then(responseBody),
    post: (url: string, body: {}) => axios.post(url, body).then(responseBody),
    put: (url: string, body: {}) => axios.put(url, body).then(responseBody),
    delete: (url: string) => axios.delete(url).then(responseBody),
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
    addItem: (productId: string, quantity = 1) => requests.put(`shoppingcart?productId=${productId}&quantity=${quantity}`, {}),
    removeItem: (productId: string, quantity = 1) => requests.delete(`shoppingcart?productId=${productId}&quantity=${quantity}`)
}

const agent = {
    Catalog,
    Orders,
    Cart: ShoppingCart
}

export default agent; 