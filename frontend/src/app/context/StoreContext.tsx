import { PropsWithChildren, createContext, useContext, useEffect, useState } from "react";
import { Cart } from "../models/cart";
import agent from "../api/agent";
import LoadingComponent from "../layout/LoadingComponent";

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
    const [cart, setCart] = useState<Cart | null>(null);
    const [loading, setLoading] = useState(true);

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

    useEffect(() => {
        agent.ShoppingCart.list()
          .then(cart => {
            console.log(`Received cart from API: ${cart}`);
            setCart(cart);
            })
          .catch(error => console.log(error))
          .finally(() => setLoading(false));
      }, [setCart]);
    
    if(loading) return <LoadingComponent message='Initialising app ...'/>
    return (
        <StoreContext.Provider value={{cart, setCart, removeItem}}>
            {children}
        </StoreContext.Provider>
    )
}