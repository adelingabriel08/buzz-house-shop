import axios from "axios";
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Product } from "../../app/models/product";
import { Divider, Grid, Table, TableBody, TableCell, TableContainer, TableRow, TextField, Typography } from "@mui/material";
import agent from "../../app/api/agent";
import { error } from "console";
import { useStoreContext } from "../../app/context/StoreContext";
import { LoadingButton } from "@mui/lab";

export default function ItemPage() {
    const {cart, setCart, removeItem} = useStoreContext();
    const {id} = useParams<{id: string}>();
    const [product, setProduct] = useState<Product | null>();
    // {
    //     id: 1,
    //     name: `product${id}`,
    //     description: `description product${id}`,
    //     price: 1000,
    //     pictureUrl: "http://picsum.photos/100",
    //     custom: false
    // }
    const [loading, setLoading] = useState(true);
    const [quantity, setQuantity] = useState(0);
    const [submitting, setSubmitting] = useState(false);
    const item = cart?.cartItems.find(item => item.product.id === product?.id);

    useEffect(() =>{
        if(item) setQuantity(item.quantity);
        agent.Catalog.details(id ?? '')
                    .then(response => setProduct(response))
                    .catch(error => console.log(error))
                    .finally(() => setLoading(false));
    }, [id, item]);

    function handleInputChange(event: any){
        if(event.target.value < 0) return;
        setQuantity(parseInt(event.target.value));
    }

    function handleUpdateCart(){
        setSubmitting(true);
        if(!item || quantity > item.quantity){
            const updatedQuantity = item ? quantity - item.quantity : quantity;
            
            agent.ShoppingCart.addItem(product?.id!, updatedQuantity)
                .then(cart => setCart(cart))
                .catch(error => console.log(error))
                .finally(() => setSubmitting(false));
        } else {
            const updatedQuantity = item.quantity - quantity;
            agent.ShoppingCart.removeItem(product?.id!, updatedQuantity)
                .then(() => removeItem(product?.id!, updatedQuantity))
                .catch(error => console.log(error))
                .finally(() => setSubmitting(false));
        }
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
                    <Grid item xs={6}>
                        <TextField variant="outlined"
                                   type="number"
                                   label='Quantity in Cart'
                                   fullWidth
                                   value={quantity}
                                   onChange={handleInputChange}/>
                    </Grid>
                    <Grid item xs={6}>
                        <LoadingButton sx={{height: '55px'}}
                                       color='primary'
                                       size="large"
                                       variant="contained"
                                       fullWidth
                                       disabled={item?.quantity === quantity || !item && quantity === 0}
                                       loading={submitting}
                                       onClick={handleUpdateCart}>
                            {item ? 'Update Quantity' : 'Add to Cart'}
                        </LoadingButton>
                    </Grid>
                </Grid>
            </Grid>
        </Grid>
    );
}