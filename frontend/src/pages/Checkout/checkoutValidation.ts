import * as yup from 'yup';

export const validationSchema = [
    yup.object({
        street: yup.string().required('Street is required'),
        number: yup.string().required('Number is required'),
        apartmentNumber: yup.string().required('Apartment number is required'),
        floor: yup.string().required('Floor is required'),
        city: yup.string().required('City is required'),
        postalCode: yup.string().required('Postal code is required'),
        country: yup.string().required('Country is required'),
    }),
    yup.object(),
    yup.object({
        nameOnCard: yup.string().required()
    })
];