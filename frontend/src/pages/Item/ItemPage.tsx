import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Product } from "../../app/models/product";
import { Divider, Grid, Table, TableBody, TableCell, TableContainer, TableRow, TextField, Typography } from "@mui/material";
import agent from "../../app/api/agent";
import { useStoreContext } from "../../app/context/StoreContext";
import { LoadingButton } from "@mui/lab";
import { CartItem } from "../../app/models/cartItem";

export default function ItemPage() {
    const {cart, setCart, removeItem} = useStoreContext();
    const {id} = useParams<{id: string}>();
    const [product, setProduct] = useState<Product | null>();
    const [loading, setLoading] = useState(true);
    const [quantity, setQuantity] = useState(0);
    const [submitting, setSubmitting] = useState(false);
    const item = cart?.cartItems?.find(item => item.product.id === product?.id);

    useEffect(() =>{
        if(item) setQuantity(item.quantity);
        agent.Catalog.details(id ?? '')
                    .then(product => setProduct(product))
                    .catch(error => console.log(error))
                    .finally(() => setLoading(false));
    }, [id, item]);

    function handleUpdateCart(product: Product){
        console.log(`Adding product to cart: ${product}`);
        setSubmitting(true);

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
            .finally(() => setSubmitting(false));
    }

    if (loading) return <h3>Loading...</h3>;
    if (!product) return <h3>Product not found</h3>;
    
    return(
        <Grid container spacing={6}>
            <Grid item xs={6}>
                <img src={product.imageUrl} alt={product.name} style={{width: '100%'}}/>
            </Grid>
            <Grid item xs={6}>
                <Typography variant="h3">
                    {product.name}
                </Typography>
                <Divider sx={{mb: 2}}/>
                <Typography variant="h4" color='secondary'>
                    {(product.price / 100).toFixed(2)}
                </Typography>
                <TableContainer>
                    <Table>
                        <TableBody>
                            <TableRow>
                                <TableCell>Name</TableCell>
                                <TableCell>{product.name}</TableCell>
                            </TableRow>
                            <TableRow>
                                <TableCell>Description</TableCell>
                                <TableCell>{product.description}</TableCell>
                            </TableRow>
                            <TableRow>
                                <TableCell>Type</TableCell>
                                <TableCell>{product.type}</TableCell>
                            </TableRow>
                            <TableRow>
                                <TableCell>Customizeable</TableCell>
                                <TableCell>{product.custom}</TableCell>
                            </TableRow>
                            <TableRow>
                                <TableCell>Quantity in stock</TableCell>
                                <TableCell>{product.stock}</TableCell>
                            </TableRow>
                        </TableBody>
                    </Table>
                </TableContainer>
                <Grid container spacing={2}>
                    <Grid item xs={12}>
                        <LoadingButton 
                            loading={submitting} 
                            onClick={() => handleUpdateCart(product)} 
                            size="large"
                            sx={{height: '55px'}}
                            variant="contained"
                            fullWidth>
                            Add to cart
                        </LoadingButton>
                    </Grid>
                </Grid>
            </Grid>
        </Grid>
    );
}