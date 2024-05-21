import axios, { AxiosResponse } from "axios";

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
    details: (id: number) => requests.get(`products/${id}`),
    update: (id:number) => requests.put(`products/${id}`, obj),
    delete : (id:number) => requests.delete(`products/${id}`)
}

const Orders = {
    list: () => requests.get('orders'),
    list_date: () => requests.get('orders/createdDate'),
    order_status: (orderStatus : string) => requests.get('orders/orderStatus/${orderStatus}'),
    detail: (id: number) => requests.get(`orders/${id}`),
    update: (id: number) => requests.put(`orders/${id}`, obj),
    delete : (id:number) => requests.delete(`orders/${id}`)

}

const Cart = {
    get: () => requests.get('cart'),
    addItem: (productId: number, quantity = 1) => requests.post(`cart?productId=${productId}&quantity=${quantity}`, {}),
    removeItem: (productId: number, quantity = 1) => requests.delete(`cart?productId=${productId}&quantity=${quantity}`)

}

const agent = {
    Catalog,
    Cart
}

export default agent; 