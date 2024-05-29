import { Cart } from "../models/cart";

export function getCookie(key: string){
    const b = document.cookie.match("(^|;)\\s*" + key + "\\s*=\\s*([^;]+)");
    return b ? b.pop() : "";
}

export function currencyFormat(amount: number) {
    return `$${(amount / 100).toFixed(2)}`;
}

export function persistIdToken(idToken : string){
    sessionStorage.setItem("googleIdToken", idToken);
}

export function getIdToken()
{
    let idToken;

    try {
        idToken = sessionStorage.getItem("googleIdToken");
    }catch (e) {
        idToken = null;
    }

    return idToken;
}

export function cleanIdToken()
{
    sessionStorage.removeItem("googleIdToken");
}

export function calculateSubtotal(cart?: Cart | null): number {
    return cart?.cartItems.reduce((sum, item) => sum + (item.quantity * item.price), 0) ?? 0;
}

export function calculateDeliveryFee(subtotal: number): number {
    return subtotal > 1000 ? 0 : 500;
}