import { Button, Grid, Typography } from "@mui/material";
import { useStoreContext } from "../../app/context/StoreContext";
import CartSummary from "./CartSummary";
import { Link } from "react-router-dom";
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
