import { Product } from "../../app/models/product";
import ProductList from "./ProductList";
import { useState } from "react";

export default function HomePage() {
    //For testing purposes only
    const[products, setProducts] = useState<Product[]>([
        {
            id: 1,
            name: "product1",
            description: "description product1",
            price: 1000,
            pictureUrl: "http://picsum.photos/100",
            brand: "brand product1"
        },
        {
            id: 2,
            name: "product2",
            description: "description product2",
            price: 2000,
            pictureUrl: "http://picsum.photos/200",
            brand: "brand product2"
        },
        {
            id: 3,
            name: "product3",
            description: "description product3",
            price: 3000,
            pictureUrl: "http://picsum.photos/300",
            brand: "brand product3"
        }
    ])
    return(
        <>
            <ProductList products={products} />
        </>
    )
}