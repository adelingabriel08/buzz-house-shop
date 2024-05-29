import { Product } from "./product";

export interface CartItem {
    product: Product;
    quantity: number;
    productSize: number;
    customDetails: string;
    price: number;
}
