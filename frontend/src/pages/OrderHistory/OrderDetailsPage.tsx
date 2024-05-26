import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Order } from "../../app/models/order";
import agent from "../../app/api/agent";
import SendIcon from '@mui/icons-material/Send';
import { LoadingButton } from "@mui/lab";
import { TableContainer, Paper, Table, TableHead, TableRow, TableCell, TableBody, Box, Collapse, List, ListItemButton, ListItemIcon, ListItemText, ListSubheader, ListItem } from "@mui/material";
import { currencyFormat } from "../../app/util/util";

export default function OrderDetails(){
    const {id} = useParams<{id: string}>();
    const [order, setOrder] = useState<Order | null>({
        id: "first order",
        createdDate: new Date('2023-05-26T10:30:00'),
        deliveryDate: new Date('2023-05-26T10:30:00'),
        orderStatus: 1,
        cart: {
                id: '3fa85f64-5717-4562-b3fc-2c963f66afa6',
                userId: 1,
                cartItems: [{
                    product: {
                        id: '3fa85f64-5717-4562-b3fc-2c963f66afa1',
                        type: 1,
                        name: "product1",
                        description: "description product1",
                        price: 1000,
                        stock: 100,
                        imageUrl: "http://picsum.photos/100",
                        custom: false
                    },
                    quantity: 10,
                    productSize: 10,
                    customDetails: 'no custom details',
                    price: 1000
                },
                {
                    product: {
                        id: '3fa85f64-5717-4562-b3fc-2c963f66afa2',
                        type: 2,
                        name: "product2",
                        description: "description product2",
                        price: 2000,
                        stock: 200,
                        imageUrl: "http://picsum.photos/200",
                        custom: false
                    },
                    quantity: 20,
                    productSize: 20,
                    customDetails: 'no custom details',
                    price: 2000
                }]
            },
        shippingAddress: {
            street: 'strada1',
            number: '1A',
            apartmentNumber: '70',
            floor: "10",
            city: "Timisoara",
            postalCode: "codPostal",
            country: "Romania"
        }
    });
    const [loading, setLoading] = useState(true);

    // useEffect(() => {
    //     agent.Order.details(id ?? '')
    //         .then(order => setOrder(order))
    //         .catch(error => console.log(error))
    //         .finally(() => setLoading(false));
    // });

    // if(loading) <h3>Loading ...</h3>

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
                            <TableCell align="right">Subtotal</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {order?.cart?.cartItems.map((cartItem) => (
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
                                <TableCell align="right">{(cartItem.price * cartItem.quantity / 100).toFixed(2)}</TableCell>
                            </TableRow>
                    ))}
                    </TableBody>
                </Table>
            </TableContainer>
            <List
            sx={{mt: 2, width: '100%', maxWidth: 360, bgcolor: 'background.paper' }}
            component="nav"
            aria-labelledby="nested-list-subheader"
            >
                <ListItem>
                    <ListItemIcon>
                    </ListItemIcon>
                    <ListItemText primary="Street" />
                </ListItem>
                <ListItem>
                    <ListItemIcon>
                    </ListItemIcon>
                    <ListItemText primary="Number" />
                </ListItem>
                <ListItem>
                    <ListItemIcon>
                    </ListItemIcon>
                    <ListItemText primary="Apartment number" />
                </ListItem>
                <ListItem>
                    <ListItemIcon>
                    </ListItemIcon>
                    <ListItemText primary="Floor" />
                </ListItem>
                <ListItem>
                    <ListItemIcon>
                    </ListItemIcon>
                    <ListItemText primary="City" />
                </ListItem>
                <ListItem>
                    <ListItemIcon>
                    </ListItemIcon>
                    <ListItemText primary="Country" />
                </ListItem>
                <ListItem>
                    <ListItemIcon>
                    </ListItemIcon>
                    <ListItemText primary="Postal Code" />
                </ListItem>
                <ListItem>
                    <ListItemIcon>
                    </ListItemIcon>
                    <ListItemText primary="Additional Details" />
                </ListItem>
            </List>
        </>
    );
}