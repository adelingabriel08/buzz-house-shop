export interface ShippingAddress{
    street: string;
    number: string;
    apartmentNumber?: string;
    floor?: string;
    additionalDetails?: string;
    city: string;
    postalCode: string;
    country: string;
}