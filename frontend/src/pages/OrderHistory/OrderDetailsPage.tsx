import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Order } from "../../app/models/order";
import { TableContainer, Paper, Table, TableHead, TableRow, TableCell, TableBody, Box, ListItemText, ListItem, Grid, Typography } from "@mui/material";
import { calculateSubtotal, currencyFormat } from "../../app/util/util";
import agent from "../../app/api/agent";

export default function OrderDetails(){
    const {id} = useParams<{id: string}>();
    const [order, setOrder] = useState<Order | null>(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        agent.Order.details(id ?? '')
            .then(order => setOrder(order))
            .catch(error => console.log(error))
            .finally(() => setLoading(false));
    }, [id, setOrder]);

    if(loading) <h3>Loading ...</h3>

    if(!order) <h3>Order not found!</h3>

    return(
        <>
            <TableContainer component={Paper}>
                <Table sx={{ minWidth: 650 }}>
                    <TableHead>
                        <TableRow>
                            <TableCell>Product</TableCell>
                            <TableCell align="right">Price</TableCell>
                            <TableCell align="center">Quantity</TableCell>
                            <TableCell align="right">Price</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {order?.cart?.cartItems?.map((cartItem) => (
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
                                <TableCell align="center"> {cartItem.quantity} </TableCell>
                                <TableCell align="right">{currencyFormat(cartItem.price * cartItem.quantity)}</TableCell>
                            </TableRow>
                    ))}
                    </TableBody>
                </Table>
            </TableContainer>
            <Grid container spacing={1}>
                <Grid item xs={6} >
                    <Paper
                    sx={{mt: 2, width: '100%', bgcolor: 'background.paper', borderRadius: 2}}
                    component="nav"
                    aria-labelledby="nested-list-subheader">
                        <ListItem>
                            <ListItemText primary={`Street: ${order?.shippingAddress?.street}`} />
                        </ListItem>
                        <ListItem>
                            <ListItemText primary={`Number: ${order?.shippingAddress?.number}`} />
                        </ListItem>
                        <ListItem>
                            <ListItemText primary={`Apartment number: ${order?.shippingAddress?.apartmentNumber}`} />
                        </ListItem>
                        <ListItem>
                            <ListItemText primary={`Floor: ${order?.shippingAddress?.floor}`} />
                        </ListItem>
                    </Paper>
                </Grid>
                <Grid item xs={6}>
                    <Paper
                    sx={{mt: 2, width: '100%', bgcolor: 'background.paper', borderRadius: 2}}
                    component="nav"
                    aria-labelledby="nested-list-subheader">
                        <ListItem>
                            <ListItemText primary={`City: ${order?.shippingAddress?.city}`} />
                        </ListItem>
                        <ListItem>
                            <ListItemText primary={`Country: ${order?.shippingAddress?.country}`} />
                        </ListItem>
                        <ListItem>
                            <ListItemText primary={`Postal Code: ${order?.shippingAddress?.postalCode}`} />
                        </ListItem>
                        <ListItem>
                            <ListItemText primary={`Total: ${currencyFormat(calculateSubtotal(order?.cart))}`}/>
                        </ListItem>
                    </Paper>
                </Grid>
            </Grid>
            <Grid container>
                <Grid item xs={12}>
                    <Paper
                        sx={{mt: 2, width: '100%', bgcolor: 'background.paper', borderRadius: 2, p: 2}}
                        component="nav"
                        aria-labelledby="nested-list-subheader">
                            <Typography>Additional Details: {order?.shippingAddress?.additionalDetails ?? 'No additional details'}</Typography>
                        </Paper>
                </Grid>
            </Grid>
        </>
    );
}