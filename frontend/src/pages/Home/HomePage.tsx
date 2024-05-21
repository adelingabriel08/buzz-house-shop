import agent from "../../app/api/agent";
import LoadingComponent from "../../app/layout/LoadingComponent";
import { Product } from "../../app/models/product";
import ProductList from "./ProductList";
import { useEffect, useState } from "react";

export default function HomePage() {
    //For testing purposes only
    // [
    //     {
    //         id: 1,
    //         name: "product1",
    //         description: "description product1",
    //         price: 1000,
    //         pictureUrl: "http://picsum.photos/100",
    //         custom: false
    //     },
    //     {
    //         id: 2,
    //         name: "product2",
    //         description: "description product2",
    //         price: 2000,
    //         pictureUrl: "http://picsum.photos/200",
    //         custom: false
    //     },
    //     {
    //         id: 3,
    //         name: "product3",
    //         description: "description product3",
    //         price: 3000,
    //         pictureUrl: "http://picsum.photos/300",
    //         custom: false
    //     }
    // ]
    const[products, setProducts] = useState<Product[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        agent.Catalog.list()
                    .then(products => setProducts(products))
                    .catch(error => console.log(error))
                    .finally(() => setLoading(false));
    });

    if(loading) return <LoadingComponent />

    return(
        <>
            <ProductList products={products} />
        </>
    );
}