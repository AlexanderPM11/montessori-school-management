import * as Yup from 'yup';




// Esquema de validación
export const validationSchema = Yup.object({
    name: Yup.string().required("Requerido"),
    description: Yup.string().required("Requerido"),
    idTeacherLead: Yup.string().required("Requerido"),
    level: Yup.array()
        .of(Yup.string())
        .min(1, "Debe seleccionar al menos una opción")
        .required("Requerido"),
    addres: Yup.string().required("Requerido"),
    capacity: Yup.number()
        .typeError("Debe ser un número")
        .required("Requerido")
        .min(1, "Debe ser al menos 1")
        .max(200, "No puede ser mayor a 200")

});