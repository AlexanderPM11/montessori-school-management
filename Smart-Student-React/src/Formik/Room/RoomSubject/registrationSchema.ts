import * as Yup from 'yup';
// Esquema de validación
export const validationSchema = Yup.object({
    idSuject: Yup.string().required("Requerido"),
    idTeacher: Yup.string().required("Requerido"),

});