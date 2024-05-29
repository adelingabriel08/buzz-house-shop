import { Cart } from "./cart";
import { ShippingAddress } from "./shippingAddress";

export interface Order{
    id: string;
    createdDate: Date;
    deliveryDate: Date;
    userId?: string;
    shippingAddress?: ShippingAddress;
    orderStatus: number;
    cart?: Cart;
}

