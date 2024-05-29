import { Checkbox, FormControlLabel, Grid, TextField, Typography } from "@mui/material";

export default function AddressForm(){
    return (
        <>
            <Typography variant="h6" gutterBottom>Shipping address</Typography>
            <Grid container spacing={3}>
                <Grid item xs={12} sm={6}>
                    <TextField required
                               id="firstName"
                               name="firstName"
                               label="First name"
                               fullWidth
                               autoComplete="given-name"
                               variant="standard"/>
                </Grid>
                <Grid item xs={12} sm={6}>
                    <TextField required
                               id="lastName"
                               name="lastName"
                               label="Last name"
                               fullWidth
                               autoComplete="family-name"
                               variant="standard"/>
                </Grid>
                <Grid item xs={12} sm={6}>
                    <TextField required
                               id="address"
                               name="address"
                               label="Address line"
                               fullWidth
                               autoComplete="shipping adress-line"
                               variant="standard"/>
                </Grid>
                <Grid item xs={12} sm={6}>
                    <TextField required
                               id="state"
                               name="state"
                               label="State/Province/Region"
                               fullWidth
                               variant="standard"/>
                </Grid>
                <Grid item xs={12} sm={6}>
                    <TextField required
                               id="zip"
                               name="zip"
                               label="Zip / Postal code"
                               fullWidth
                               autoComplete="shipping postal-code"
                               variant="standard"/>
                </Grid>
                <Grid item xs={12} sm={6}>
                    <TextField required
                               id="country"
                               name="country"
                               label="Country"
                               fullWidth
                               autoComplete="shipping country"
                               variant="standard"/>
                </Grid>
                <Grid item xs={12}>
                    <FormControlLabel
                        control={<Checkbox color="secondary" name="saveAddress" value="yes" />}
                        label="Use this address for payment details" />
                </Grid>
            </Grid>
        </>

    );
}