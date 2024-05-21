import { Avatar, Button, Card, CardActions, CardContent, CardMedia, Typography, CardHeader } from "@mui/material";
import { Product } from "../../app/models/product";
import { Link } from "react-router-dom";
import { useState } from "react";
import agent from "../../app/api/agent";
import { LoadingButton } from "@mui/lab";
import { useStoreContext } from "../../app/context/StoreContext";

interface Props{
    product: Product;
}

export default function ProductCard({product} : Props){
    const [loading, setLoading] = useState(false);
    const {setCart} = useStoreContext();

    function handleAddItem(productId: string) {
        setLoading(true);
        // agent.Cart.addItem(productId)
        //         .then(cart => setCart(cart))
        //         .catch(error => console.log(error))
        //         .finally(() => setLoading(false));
        setCart({
            id: 1,
            userId: 'hackerId',
            items: [{
                productId: 1,
                name: "product1",
                price: 1000,
                pictureUrl: "http://picsum.photos/100",
                quantity: 4
            },
            {
                productId: 2,
                name: "product2",
                price: 2000,
                pictureUrl: "http://picsum.photos/200",
                quantity: 5
            },
            {
                productId: 3,
                name: "product3",
                price: 3000,
                pictureUrl: "http://picsum.photos/300",
                quantity: 6
            }]
          });
        setLoading(false);
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
                    ${(product.price / 100).toFixed(2)}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                    {product.type}
                </Typography>
            </CardContent>
            <CardActions>
                <LoadingButton loading={loading} 
                               onClick={() => handleAddItem(product.id)} 
                               size="small">
                    Add to cart
                </LoadingButton>
                <Button component={Link} to={`/item/${product.id}`} size="small">View</Button>
            </CardActions>
        </Card>
    )
}