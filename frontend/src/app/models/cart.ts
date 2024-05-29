import { CartItem } from "./cartItem";

export interface Cart {
    id: string;
    userId: number;
    cartItems: CartItem[];
}