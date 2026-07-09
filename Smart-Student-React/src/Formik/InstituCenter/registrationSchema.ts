import * as Yup from "yup";

const formatTel = /^\(\d{3}\)-\d{3}-\d{4}$/; // Formato esperado: (XXX)-XXX-XXXX

export const validationSchema = Yup.object({
    name: Yup.string().required("El nombre es requerido"),
    address: Yup.string().required("La dirección es requerida"),
    // nameMunicipality: Yup.string().required("El municipio es requerido"),
    phone: Yup.string()
        .matches(formatTel, "Número de teléfono no válido. Ej: 809-555-1234")
        .required("El teléfono es requerido"),
    mobile: Yup.string()
        .matches(formatTel, "Número de móvil no válido. Ej: 809-555-5678")
        .required("El móvil es requerido"),
    idRector: Yup.string().required("Debe seleccionar un rector"),
    idCordinator: Yup.string().required("Debe seleccionar un coordinador"),
    // idSecretary: Yup.string().required("Debe seleccionar un secretario"),
    idProvinceDom: Yup.string().required("La provincia es requerida"),
    session: Yup.string().required("Debe seleccionar una tanda"),
    educationalRegistry: Yup.string().required("El código del centro es requerido"),
    district: Yup.string().required("El distrito es requerido"),
    regional: Yup.string().required("La regional es requerida"),

    // Campos opcionales
    website: Yup.string().url("Debe ser una URL válida").nullable(),
    academicResolution: Yup.string().nullable(),
});
