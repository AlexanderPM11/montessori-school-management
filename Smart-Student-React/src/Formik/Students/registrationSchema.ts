import * as Yup from 'yup';



const formatTel = /^\(\d{3}\)-\d{3}-\d{4}$/; // Formato esperado: (XXX)-XXX-XXXX

// Esquema de validación
export const validationSchema = Yup.object({
    name: Yup.string().required("Requerido"),
    lastname: Yup.string().required("Requerido"),
    sexo: Yup.string().required("Requerido"),
    idGrade: Yup.string().required("Requerido"),
    idNacionality: Yup.string().required("Requerido"),
    bornDate: Yup.date().required("Requerido"),
    direction: Yup.string().required("Requerido"),
    idTypeRegister: Yup.string().required("Requerido"),
    relationship: Yup.string().required("Requerido"),
    idFather: Yup.string().required("Requerido"),
    idMother: Yup.string().required("Requerido"),
    telFather: Yup.string()
        .matches(formatTel, "Número de Teléfono inválido.")
        .required("Requerido"),
    telMother: Yup.string()
        .matches(formatTel, "Número de Teléfono inválido.")
        .required("Requerido"),
    emergencyPerson: Yup.string().required("Requerido"),
    emergencyTel: Yup.string()
        .matches(formatTel, "Número de Teléfono inválido.")
        .required("Requerido"),

});