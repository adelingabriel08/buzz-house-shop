import { Grid, Typography } from "@mui/material";
import { useFormContext } from "react-hook-form";
import AppTextInput from "../../app/components/AppTextInput";
import AppCheckbox from "../../app/components/AppCheckbox";

export default function AddressForm(){
    const {control, handleSubmit} = useFormContext();
    return (
        <>
            <Typography variant="h6" gutterBottom>Shipping address</Typography>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <AppTextInput control={control} name="street" label="Street" />
                </Grid>
                <Grid item xs={12} sm={4}>
                    <AppTextInput control={control} name="number" label="Number" />
                </Grid>
                <Grid item xs={12} sm={4}>
                    <AppTextInput control={control} name="apartmentNumber" label="ApartmentNumber" />
                </Grid>
                <Grid item xs={12} sm={4}>
                    <AppTextInput control={control} name="floor" label="Floor" />
                </Grid>
                <Grid item xs={12} sm={6}>
                    <AppTextInput control={control} name="city" label="City" />
                </Grid>
                <Grid item xs={12} sm={6}>
                    <AppTextInput control={control} name="postalCode" label="PostalCode" />
                </Grid>
                <Grid item xs={12} sm={6}>
                    <AppTextInput control={control} name="country" label="Country" />
                </Grid>
                <Grid item xs={12}>
                    <AppTextInput control={control} name="additionalDetails" label="AdditionalDetails" />
                </Grid>
                <Grid item xs={12}>
                    <AppCheckbox name='saveAddress' label="Save this as the default address" control={control} />
                </Grid>
            </Grid>
        </>

    );
}