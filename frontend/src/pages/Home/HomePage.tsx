import { Avatar, List, ListItem, ListItemAvatar, ListItemText } from "@mui/material";
import { Product } from "../../app/models/product";
import ProductList from "./ProductList";
interface Props{
    products: Product[];
}
export default function HomePage({products}: Props) {
    return(
        <>
            <ProductList products={products} />
        </>
    )
}