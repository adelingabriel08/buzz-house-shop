import { Cart } from "./cart";

export interface Order{
    id: string;
    createdDate: Date;
    deliveryDate: Date;
    userId?: string;
    shippingAddress?: ShippingAddress;
    orderStatus: number;
    cart?: Cart;
}

export interface ShippingAddress{
    street: string;
    number: string;
    apartmentNumber?: string;
    floor?: string;
    additionalDetails?: string;
    city: string;
    postalCode: string;
    country: string;
}