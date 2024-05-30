import { Remove, Add, Delete } from "@mui/icons-material";
import { LoadingButton } from "@mui/lab";
import { TableContainer, Paper, Table, TableHead, TableRow, TableCell, TableBody, Box } from "@mui/material";
import { currencyFormat } from "../../app/util/util";
import { useStoreContext } from "../../app/context/StoreContext";
import { CartItem } from "../../app/models/cartItem";
import agent from "../../app/api/agent";
import { useState } from "react";

interface Props {
    isCart?: boolean;
}
export default function CartTable({isCart=true}: Props){
    const {cart, setCart} = useStoreContext();
    const [status, setStatus] = useState({
        loading: false,
        name: ''
    });
    
    function handleAddItem(cartItem: CartItem) {
        setStatus({loading: true, name: 'add' + cartItem.product.id});
        cartItem.quantity += 1;
        if(cart)
            agent.ShoppingCart.updateCartitem(cart.id, cartItem)
                .then(cart => setCart(cart))
                .catch(error => console.log(error))
                .finally(() => setStatus({loading: false, name: ''}));
        setStatus({loading: false, name: ''});
    }

    function handleRemoveItem(cartItem: CartItem) {
        setStatus({loading: true, name: 'rem' + cartItem.product.id});
        if(cartItem.quantity === 0 ) cartItem.quantity -= 1;
        if(cart){
            if(cartItem.quantity - 1 > 0){
                cartItem.quantity -= 1;
                agent.ShoppingCart.updateCartitem(cart.id, cartItem)
                    .then(cart => setCart(cart))
                    .catch(error => console.log(error))
                    .finally(() => setStatus({loading: false, name: ''}));
            } else{
                agent.ShoppingCart.removeCartItem(cart.id, cartItem)
                    .then(cart => setCart(cart))
                    .catch(error => console.log(error))
                    .finally(() => setStatus({loading: false, name: ''}));
            }
        }
        setStatus({loading: false, name: ''});
    }

    function handleDeleteItem(cartItem: CartItem) {
        setStatus({loading: true, name: 'del' + cartItem.product.id});
        if(cart)
            agent.ShoppingCart.removeCartItem(cart.id, cartItem)
                .then(cart => setCart(cart))
                .catch(error => console.log(error))
                .finally(() => setStatus({loading: false, name: ''}));
        setStatus({loading: false, name: ''});
    }

    return (
        <TableContainer component={Paper}>
            <Table sx={{ minWidth: 650 }}>
                <TableHead>
                    <TableRow>
                        <TableCell>Product</TableCell>
                        <TableCell align="right">Price</TableCell>
                        <TableCell align="center">Quantity</TableCell>
                        <TableCell align="right">Subtotal</TableCell>
                        {isCart &&
                        <TableCell align="right"></TableCell>}
                    </TableRow>
                </TableHead>
                <TableBody>
                    {cart?.cartItems?.map((cartItem) => (
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
                            {isCart &&
                                <LoadingButton loading={status.loading && status.name === 'rem' + cartItem.product.id} 
                                            onClick={() => handleRemoveItem(cartItem)} 
                                            color="error">
                                    <Remove />
                                </LoadingButton>
                            }
                                {cartItem.quantity}
                                
                            {isCart &&
                                <LoadingButton loading={status.loading && status.name === 'add' + cartItem.product.id}
                                            onClick={() => handleAddItem(cartItem)} 
                                            color="secondary">
                                    <Add />
                                </LoadingButton>
                            }
                            </TableCell>
                            <TableCell align="right">{(cartItem.price * cartItem.quantity / 100).toFixed(2)}</TableCell>
                            {isCart &&
                            <TableCell align="right">
                                <LoadingButton loading={status.loading && status.name === 'del' + cartItem.product.id} 
                                            onClick={() => handleDeleteItem(cartItem)} 
                                            color="error">
                                    <Delete />
                                </LoadingButton>
                            </TableCell>
                            }
                        </TableRow>
                ))}
                </TableBody>
            </Table>
        </TableContainer>
    );
}