import Swal from "sweetalert2";

interface Props {
    title: string;
    description?: string;
    htmlContent?: string; // 🔹 Nuevo: contenido HTML personalizado
    type: 'success' | 'error' | 'warning' | 'info' | 'question';
    showCancelButton?: boolean;
    confirmButtonText?: string;
    cancelButtonText?: string;
    onConfirm?: () => void;
}

export const SweetAlertCustom = async ({
    type,
    title,
    description,
    htmlContent,
    showCancelButton = false,
    confirmButtonText = "Aceptar",
    cancelButtonText = "Cancelar",
    onConfirm,
}: Props) => {
    Swal.fire({
        title: title || "Alert",
        text: !htmlContent ? description : undefined,
        html: htmlContent, // Si se pasa html, reemplaza text
        icon: type || "info",
        showCancelButton,
        confirmButtonText,
        cancelButtonText,
    }).then(async (result) => {
        if (result.isConfirmed && onConfirm) {
            Swal.close();
            onConfirm();
        }
    });
};
