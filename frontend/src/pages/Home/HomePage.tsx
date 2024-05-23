import agent from "../../app/api/agent";
import LoadingComponent from "../../app/layout/LoadingComponent";
import { Product } from "../../app/models/product";
import ProductList from "./ProductList";
import { useEffect, useState } from "react";

export default function HomePage() {

    //For testing purposes only
    // const productsTesting: Product[] = [
    //     {
    //         id: 'some-product-id1',
    //         type: 1,
    //         name: "product1",
    //         description: "description product1",
    //         price: 1000,
    //         stock: 100,
    //         imageUrl: "http://picsum.photos/100",
    //         custom: false
    //     },
    //     {
    //         id: 'some-product-id2',
    //         type: 2,
    //         name: "product2",
    //         description: "description product2",
    //         price: 2000,
    //         stock: 200,
    //         imageUrl: "http://picsum.photos/200",
    //         custom: false
    //     },
    //     {
    //         id: 'some-product-id3',
    //         type: 3,
    //         name: "product3",
    //         description: "description product3",
    //         price: 3000,
    //         stock: 300,
    //         imageUrl: "http://picsum.photos/300",
    //         custom: false
    //     }
    // ]
    // console.log('Creating products');
    // productsTesting.forEach(product => agent.Catalog.create(product));
    // console.log('Done creating products');

    const[products, setProducts] = useState<Product[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        agent.Catalog.list()
            .then(products => setProducts(products))
            .catch(error => console.log(error))
            .finally(() => setLoading(false));
    }, [setProducts]);

    if(loading) return <LoadingComponent />

    return(
        <>
            <ProductList products={products} />
        </>
    );
}