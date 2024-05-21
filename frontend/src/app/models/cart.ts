export interface CartItem {
    productId: number;
    name: string;
    price: number;
    pictureUrl: string;
    type?: string;
    quantity: number;
}

export interface Cart {
    id: number;
    userId: string;
    items: CartItem[];
}