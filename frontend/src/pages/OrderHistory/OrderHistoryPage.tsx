import { TableContainer, Paper, Table, TableHead, TableRow, TableCell, TableBody, Button } from "@mui/material";
import { Order } from "../../app/models/order";
import { useState } from "react";
import { Link } from "react-router-dom";

export default function OrderHistory() {
    const [orders, setOrders] = useState<Order[]>([{
        id: "first-order",
        createdDate: new Date('2023-05-26T10:30:00'),
        deliveryDate: new Date('2023-05-26T10:30:00'),
        orderStatus: 1
    }]);
    const [loading, setLoading] = useState(true);

    // useEffect(() => {
    //     agent.Order.list()
    //         .then(orders => setOrders(orders))
    //         .catch(error => console.log(error))
    //         .finally(() => setLoading(false));
    // });

    return(
        <>
            <TableContainer component={Paper}>
                <Table sx={{ minWidth: 650 }}>
                    <TableHead>
                        <TableRow>
                            <TableCell>Order</TableCell>
                            <TableCell align="right">Created on</TableCell>
                            <TableCell align="center">Delivered on</TableCell>
                            <TableCell align="right">Order Status</TableCell>
                            <TableCell align="right"></TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {orders.map((order) => (
                            <TableRow
                            key={order.id}
                            sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                            >
                                <TableCell component="th" scope="row"> {order.id} </TableCell>
                                <TableCell align="right"> {order.createdDate.toDateString()} </TableCell>
                                <TableCell align="center"> {order.deliveryDate.toDateString()}</TableCell>
                                <TableCell align="right"> {order.orderStatus.toString()}</TableCell>
                                <TableCell align="right">
                                    <Button component={Link} to={`order/${order.id}`}>
                                        Show more details
                                    </Button>
                                </TableCell>
                            </TableRow>
                    ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </>
    );
}