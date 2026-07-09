import * as Yup from 'yup';

const passwordValidation = Yup.string()
    .matches(
        /^(?=.*[A-Z])/, // Al menos una letra mayúscula
        "Debe contener al menos una letra mayúscula"
    )
    .matches(
        /^(?=.*[a-z])/, // Al menos una letra minúscula
        "Debe contener al menos una letra minúscula"
    )
    .matches(
        /^(?=.*[0-9])/, // Al menos un número
        "Debe contener al menos un número"
    )
    .matches(
        /^(?=.*[!@#$%^&*(),.?":{}|<>])/, // Al menos un carácter especial
        "Debe contener al menos un carácter especial"
    )
    .min(8, "La contraseña debe tener al menos 8 caracteres");
const cardIdRegExp = /^(\d{3})-(\d{7})-(\d{1})$/; // Formato esperado: XXX-XXXXXXX-X
const formatTel = /^\(\d{3}\)-\d{3}-\d{4}$/; // Formato esperado: (XXX)-XXX-XXXX

// Esquema de validación
export const validationSchema = Yup.object({
    name: Yup.string().required("Requerido"),
    lastname: Yup.string().required("Requerido"),
    userName: Yup.string().required("Requerido"),
    cardId: Yup.string()
        .matches(cardIdRegExp, "Número de cédula inválido.")
        .required("Requerido"),
    addres: Yup.string().required("Requerido"),
    dateBirth: Yup.date().required("Requerido"),
    gender: Yup.string().required("Requerido"),
    educationLevel: Yup.string().required("Requerido"),
    relationship: Yup.string().required("Requerido"),
    phone: Yup.string()
        .matches(formatTel, "Número de Teléfono inválido.")
        .required("Requerido"),
    civilStatus: Yup.string().required("Requerido"),
    nationality: Yup.string().required("Requerido"),
    password: passwordValidation,
    confirmPassword: Yup.string().oneOf(
        [Yup.ref("password"), ""],
        "Las contraseñas deben coincidir"
    ),
    roles: Yup.array()
        .of(Yup.string())
        .min(1, "Debe seleccionar al menos un Rol")
        .required("Requerido"),
});