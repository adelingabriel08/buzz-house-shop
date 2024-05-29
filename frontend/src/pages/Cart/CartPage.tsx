import { useEffect, useState } from "react";
import { Cart, CartItem } from "../../app/models/cart";
import agent from "../../app/api/agent";
import LoadingComponent from "../../app/layout/LoadingComponent";
import { Box, Button, Grid, Icon, IconButton, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography } from "@mui/material";
import { Add, Delete, Remove } from "@mui/icons-material";
import { useStoreContext } from "../../app/context/StoreContext";
import { LoadingButton } from "@mui/lab";
import CartSummary from "./CartSummary";
import { currencyFormat } from "../../app/util/util";
import { Link } from "react-router-dom";
import { Product } from "../../app/models/product";

export default function CartPage() {
    const {cart, setCart, removeItem} = useStoreContext();
    const [status, setStatus] = useState({
        loading: false,
        name: ''
    });

    function handleAddItem(cartItem: CartItem) {
        setStatus({loading: true, name: 'add' + cartItem.product.id});
        console.log('Adding Item in basket');
        console.log(`${cartItem.product ? "product exists" : "product doesn't exist"}`)
        if(cart)
            agent.ShoppingCart.addItem(cart.id, cartItem)
                .then(cart => setCart(cart))
                .catch(error => console.log(error))
                .finally(() => setStatus({loading: false, name: ''}));
        setStatus({loading: false, name: ''});
    }

    function handleRemoveItem(productId: string, quantity = 1, name: string) {
        setStatus({loading: true, name});
        console.log('Removing Item from basket')
        if(cart)
            agent.ShoppingCart.removeItem(cart.id, productId)
                .then(cart => setCart(cart))
                .catch(error => console.log(error))
                .finally(() => setStatus({loading: false, name: ''}));
        setStatus({loading: false, name: ''});
    }

    if(!cart) return <Typography variant="h3">Your cart is empty</Typography>

    return(
        <>
            <TableContainer component={Paper}>
                <Table sx={{ minWidth: 650 }}>
                    <TableHead>
                        <TableRow>
                            <TableCell>Product</TableCell>
                            <TableCell align="right">Price</TableCell>
                            <TableCell align="center">Quantity</TableCell>
                            <TableCell align="right">Subtotal</TableCell>
                            <TableCell align="right"></TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {cart.cartItems.map((cartItem) => (
                            <TableRow
                            key={cartItem.product.id}
                            sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                            >
                                <TableCell component="th" scope="row">
                                    <Box display='flex' alignItems='center'>
                                        <img src={cartItem.product.imageUrl} alt={cartItem.product.name} style={{height: 50, marginRight:20}} />
                                        <span>{cartItem.product.name}</span>
                                    </Box>
                                </TableCell>
                                <TableCell align="right">{currencyFormat(cartItem.price)}</TableCell>
                                <TableCell align="center">
                                    <LoadingButton loading={status.loading && status.name === 'rem' + cartItem.product.id} 
                                                onClick={() => handleRemoveItem(cartItem.product.id, 1, 'rem' + cartItem.product.id)} 
                                                color="error">
                                        <Remove />
                                    </LoadingButton>
                                    {cartItem.quantity}
                                    <LoadingButton loading={status.loading && status.name === 'add' + cartItem.product.id}
                                                onClick={() => handleAddItem(cartItem)} 
                                                color="secondary">
                                        <Add />
                                    </LoadingButton>
                                </TableCell>
                                <TableCell align="right">{(cartItem.price * cartItem.quantity / 100).toFixed(2)}</TableCell>
                                <TableCell align="right">
                                    <LoadingButton loading={status.loading && status.name === 'del' + cartItem.product.id} 
                                                onClick={() => handleRemoveItem(cartItem.product.id, cartItem.quantity, 'del' + cartItem.product.id)} 
                                                color="error">
                                        <Delete />
                                    </LoadingButton>
                                </TableCell>
                            </TableRow>
                    ))}
                    </TableBody>
                </Table>
            </TableContainer>
            <Grid container>
                <Grid item xs={6} />
                <Grid item xs={6} >
                    <CartSummary />
                    <Button component={Link}
                            to='/checkout'
                            variant='contained'
                            size='large'
                            fullWidth>
                        Checkout
                    </Button>
                </Grid>
            </Grid>
        </>
    );
}
