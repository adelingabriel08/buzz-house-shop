import { Avatar, Button, Card, CardActions, CardContent, CardMedia, Typography, CardHeader } from "@mui/material";
import { Product } from "../../app/models/product";
import { Link } from "react-router-dom";
import { useState } from "react";
import agent from "../../app/api/agent";
import { LoadingButton } from "@mui/lab";
import { useStoreContext } from "../../app/context/StoreContext";
import { currencyFormat } from "../../app/util/util";
import { Cart } from "../../app/models/cart";
import { CartItem } from "../../app/models/cartItem";

interface Props{
    product: Product;
}

export default function ProductCard({product} : Props){
    const [loading, setLoading] = useState(false);
    const {cart, setCart} = useStoreContext();

    function handleAddItem(product: Product) {
        console.log(`Adding product to cart: ${product}`);
        setLoading(true);

        const cartItem: CartItem = {
            product: product,
            quantity: 1,
            productSize: product.type,
            customDetails: product.description,
            price: product.price
        }
        
        if(cart)
            agent.ShoppingCart.addItem(cart.id, cartItem)
            .then(cart => setCart(cart))
            .catch(error => console.log(error))
            .finally(() => setLoading(false));
    }

    return(
        <Card>
            <CardHeader 
                avatar={
                    <Avatar sx={{bgcolor: 'secondary.main'}}>
                        {product.name.charAt(0).toUpperCase()}
                    </Avatar>
                } 
                title={product.name}
                titleTypographyProps={{
                    sx: {fontWeight: 'bold', color: 'primary.main'}
                }}/>
            <CardMedia
                sx={{ height: 140, backgroundSize: 'contain', bgcolor: 'primary.light' }}
                image={product.imageUrl}
                title={product.name}/>
            <CardContent>
                <Typography gutterBottom color="secondary" variant="h5">
                    {currencyFormat(product.price)}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                    {product.type}
                </Typography>
            </CardContent>
            <CardActions>
                <LoadingButton loading={loading} 
                               onClick={() => handleAddItem(product)} 
                               size="small">
                    Add to cart
                </LoadingButton>
                <Button component={Link} to={`/item/${product.id}`} size="small">View</Button>
            </CardActions>
        </Card>
    )
}