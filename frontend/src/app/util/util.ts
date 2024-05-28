import {Simulate} from "react-dom/test-utils";
import canPlayThrough = Simulate.canPlayThrough;

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