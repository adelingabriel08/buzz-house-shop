import { Grid, Typography } from "@mui/material";
import CartSummary from "../Cart/CartSummary";
import CartTable from "../Cart/CartTable";

export default function Review(){
    return (
        <>
            <Typography variant="h6" gutterBottom>
                Order summary
            </Typography>
            <CartTable isCart={false} />
            <Grid container>
                <Grid item xs={6} />
                <Grid item xs={6} >
                    <CartSummary />
                </Grid>
            </Grid>
        </>
    );
}