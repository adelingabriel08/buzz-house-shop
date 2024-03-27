import { Avatar, List, ListItem, ListItemAvatar, ListItemText } from "@mui/material";
import { Product } from "../../app/models/product";
interface Props{
    products: Product[];
}
export default function HomePage({products}: Props) {
    return(
        <>
            <List>
                {products.map((product) => (
                    <ListItem key={product.id}>
                        <ListItemAvatar>
                            <Avatar src={product.pictureUrl}/>
                        </ListItemAvatar>
                        <ListItemText>
                            {product.name} - {product.price}
                        </ListItemText>
                    </ListItem>
                ))}
            </List>
        </>
    )
}