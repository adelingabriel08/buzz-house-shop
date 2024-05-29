import { useState } from "react";
import agent from "../../app/api/agent";
import { Box, Button, Grid, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography } from "@mui/material";
import { Add, Delete, Remove } from "@mui/icons-material";
import { useStoreContext } from "../../app/context/StoreContext";
import { LoadingButton } from "@mui/lab";
import CartSummary from "./CartSummary";
import { currencyFormat } from "../../app/util/util";
import { Link } from "react-router-dom";
import { CartItem } from "../../app/models/cartItem";
import CartTable from "./CartTable";

export default function CartPage() {
    const {cart, setCart, removeItem} = useStoreContext();

    if(!cart) return <Typography variant="h3">Your cart is empty</Typography>

    return(
        <>
            <CartTable />
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
