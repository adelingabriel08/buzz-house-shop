import { useEffect, useState } from "react";
import { Cart } from "../../app/models/cart";
import agent from "../../app/api/agent";
import LoadingComponent from "../../app/layout/LoadingComponent";
import { Box, Button, Grid, Icon, IconButton, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography } from "@mui/material";
import { Add, Delete, Remove } from "@mui/icons-material";
import { useStoreContext } from "../../app/context/StoreContext";
import { LoadingButton } from "@mui/lab";
import CartSummary from "./CartSummary";
import { currencyFormat } from "../../app/util/util";
import { Link } from "react-router-dom";

export default function CartPage() {
    const {cart, setCart, removeItem} = useStoreContext();
    const [status, setStatus] = useState({
        loading: false,
        name: ''
    });

    function handleAddItem(productId: string, name: string) {
        setStatus({loading: true, name});
        console.log('Adding Item in basket');
        agent.Cart.addItem(productId)
            .then(cart => setCart(cart))
            .catch(error => console.log(error))
            .finally(() => setStatus({loading: true, name: ''}));
        setStatus({loading: true, name: ''});
    }

    function handleRemoveItem(productId: string, quantity = 1, name: string) {
        setStatus({loading: true, name});
        console.log('Removing Item from basket')
        if(cart){
            agent.Cart.removeItem(productId, quantity)
                .then(cart => setCart(cart))
                .catch(error => console.log(error))
                .finally(() => setStatus({loading: true, name: ''}));
        }
        setStatus({loading: true, name: ''});
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
                        {cart.cartItems.map((item) => (
                            <TableRow
                            key={item.product.id}
                            sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                            >
                                <TableCell component="th" scope="row">
                                    <Box display='flex' alignItems='center'>
                                        <img src={item.product.imageUrl} alt={item.product.name} style={{height: 50, marginRight:20}} />
                                        <span>{item.product.name}</span>
                                    </Box>
                                </TableCell>
                                <TableCell align="right">{currencyFormat(item.price)}</TableCell>
                                <TableCell align="center">
                                    <LoadingButton loading={status.loading && status.name === 'rem' + item.product.id} 
                                                onClick={() => handleRemoveItem(item.product.id, 1, 'rem' + item.product.id)} 
                                                color="error">
                                        <Remove />
                                    </LoadingButton>
                                    {item.quantity}
                                    <LoadingButton loading={status.loading && status.name === 'add' + item.product.id}
                                                onClick={() => handleAddItem(item.product.id, 'add' + item.product.id)} 
                                                color="secondary">
                                        <Add />
                                    </LoadingButton>
                                </TableCell>
                                <TableCell align="right">{(item.price * item.quantity / 100).toFixed(2)}</TableCell>
                                <TableCell align="right">
                                    <LoadingButton loading={status.loading && status.name === 'del' + item.product.id} 
                                                onClick={() => handleRemoveItem(item.product.id, item.quantity, 'del' + item.product.id)} 
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
