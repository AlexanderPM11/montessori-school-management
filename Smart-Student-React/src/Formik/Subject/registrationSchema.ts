import * as Yup from 'yup';




// Esquema de validación
export const validationSchema = Yup.object({
    name: Yup.string().required("Requerido"),
    description: Yup.string().required("Requerido"),

});