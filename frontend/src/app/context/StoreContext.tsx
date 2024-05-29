import { PropsWithChildren, createContext, useContext, useState } from "react";
import { Cart } from "../models/cart";

interface StoreContextValue {
    cart: Cart | null;
    setCart: (cart: Cart) => void;
    removeItem: (productId: string, quantity: number) => void;
}

export const StoreContext = createContext<StoreContextValue | undefined>(undefined);

export function useStoreContext(){
    const context = useContext(StoreContext);

    if(context === undefined) throw Error('We do not seem to be inside the provider');

    return context;
}

export function StoreProvider({children}: PropsWithChildren<any>){
    console.log("Entered StoreProvider method.")
    const [cart, setCart] = useState<Cart | null>(null);
    // {
    //     id: '3fa85f64-5717-4562-b3fc-2c963f66afa6',
    //     userId: 1,
    //     cartItems: [{
    //         product: {
    //             id: '3fa85f64-5717-4562-b3fc-2c963f66afa1',
    //             type: 1,
    //             name: "product1",
    //             description: "description product1",
    //             price: 1000,
    //             stock: 100,
    //             imageUrl: "http://picsum.photos/100",
    //             custom: false
    //         },
    //         quantity: 10,
    //         productSize: 10,
    //         customDetails: 'no custom details',
    //         customImg: 'nocustomimg',
    //         price: 1000
    //     },
    //     {
    //         product: {
    //             id: '3fa85f64-5717-4562-b3fc-2c963f66afa2',
    //             type: 2,
    //             name: "product2",
    //             description: "description product2",
    //             price: 2000,
    //             stock: 200,
    //             imageUrl: "http://picsum.photos/200",
    //             custom: false
    //         },
    //         quantity: 20,
    //         productSize: 20,
    //         customDetails: 'no custom details',
    //         customImg: 'nocustomimg',
    //         price: 2000
    //     }]
    // }
    function removeItem(productId: string, quantity: number){
        if (!cart) return;
        const items = [...cart.cartItems];
        const itemIndex = items.findIndex(item => item.product.id === productId);
        if (itemIndex >= 0){
            items[itemIndex].quantity -= quantity;
            if(items[itemIndex].quantity === 0) items.splice(itemIndex, 1);
            setCart(prevState => {
                return {...prevState!, cartItems: items}
            })
        }
    }

    return (
        <StoreContext.Provider value={{cart, setCart, removeItem}}>
            {children}
        </StoreContext.Provider>
    )
}