import axios from "axios";
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Product } from "../../app/models/product";
import { Divider, Grid, Table, TableBody, TableCell, TableContainer, TableRow, Typography } from "@mui/material";
import agent from "../../app/api/agent";
import { error } from "console";

export default function ItemPage() {
    const {id} = useParams<{id: string}>();
    const [product, setProduct] = useState<Product | null>();
    // {
    //     id: 1,
    //     name: `product${id}`,
    //     description: `description product${id}`,
    //     price: 1000,
    //     pictureUrl: "http://picsum.photos/100",
    //     custom: false
    // }
    const [loading, setLoading] = useState(true);

    useEffect(() =>{
        agent.Catalog.details(parseInt(id ?? "1"))
                    .then(response => setProduct(response))
                    .catch(error => console.log(error))
                    .finally(() => setLoading(false));
    }, [id]);

    if (loading) return <h3>Loading...</h3>;
    if (!product) return <h3>Product not found</h3>;
    
    return(
        <Grid container spacing={6}>
            <Grid item xs={6}>
                <img src={product.imageUrl} alt={product.name} style={{width: '100%'}}/>
            </Grid>
            <Grid item xs={6}>
                <Typography variant="h3">
                    {product.name}
                </Typography>
                <Divider sx={{mb: 2}}/>
                <Typography variant="h4" color='secondary'>
                    {(product.price / 100).toFixed(2)}
                </Typography>
                <TableContainer>
                    <Table>
                        <TableBody>
                            <TableRow>
                                <TableCell>Name</TableCell>
                                <TableCell>{product.name}</TableCell>
                            </TableRow>
                            <TableRow>
                                <TableCell>Description</TableCell>
                                <TableCell>{product.description}</TableCell>
                            </TableRow>
                            <TableRow>
                                <TableCell>Type</TableCell>
                                <TableCell>{product.type}</TableCell>
                            </TableRow>
                            <TableRow>
                                <TableCell>Customizeable</TableCell>
                                <TableCell>{product.custom}</TableCell>
                            </TableRow>
                            <TableRow>
                                <TableCell>Quantity in stock</TableCell>
                                <TableCell>{product.stock}</TableCell>
                            </TableRow>
                        </TableBody>
                    </Table>
                </TableContainer>
            </Grid>
        </Grid>
    );
}