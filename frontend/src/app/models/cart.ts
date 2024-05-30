import { CartItem } from "./cartItem";

export interface Cart {
    id: string;
    userId: string;
    cartItems: CartItem[];
}