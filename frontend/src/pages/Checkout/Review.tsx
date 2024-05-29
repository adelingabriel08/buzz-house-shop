import { Grid, List, ListItem, ListItemText, Typography } from "@mui/material";
import { Fragment } from "react/jsx-runtime";

const addresses = ['1 MUI Drive', 'Reactville', 'Anytown', '99999', 'USA'];
const payments = [
  { name: 'Card type:', detail: 'Visa' },
  { name: 'Card holder:', detail: 'Mr. John Smith' },
  { name: 'Card number:', detail: 'xxxx-xxxx-xxxx-1234' },
  { name: 'Expiry date:', detail: '04/2024' },
];

export default function Review(){
    return (
        <>
            <Typography variant="h6" gutterBottom>
                Order summary
            </Typography>
            <List disablePadding>
                <ListItem sx={{ py: 1, px: 0}}>
                    <ListItemText primary="Total"/>
                    <Typography variant="subtitle1" sx={{fontWeight: 700}}>
                        $34.06
                    </Typography>
                </ListItem>
            </List>
            <Grid container spacing={2}>
                <Grid item xs={12} sm={6}>
                    <Typography variant="h6" gutterBottom sx={{mt: 2}}>
                        Shipping
                    </Typography>
                    <Typography gutterBottom>John Smith</Typography>
                    <Typography gutterBottom>{addresses.join(', ')}</Typography>
                </Grid>
                <Grid item container direction="column" xs={12} sm={6}>
                    <Typography variant="h6" gutterBottom sx={{mt: 2}}>
                        Payment details
                    </Typography>
                    <Grid container>
                        {payments.map((payment) => (
                            <Fragment key={payment.name}>
                                <Grid item xs={6}>
                                    <Typography gutterBottom>{payment.name}</Typography>
                                </Grid>
                                <Grid item xs={6}>
                                    <Typography gutterBottom>{payment.detail}</Typography>
                                </Grid>
                            </Fragment>
                        ))}
                    </Grid>
                </Grid>
            </Grid>
        </>
    );
}