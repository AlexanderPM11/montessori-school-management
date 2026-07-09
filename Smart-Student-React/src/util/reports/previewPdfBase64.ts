import { ToastifyCustom } from "../ToastifyCustom ";

export const previewBase64Pdf = (base64: string) => {
    if (!base64) {
        alert("No se proporcionó contenido para la vista previa.");
        return;
    }

    try {
        const byteCharacters = atob(base64);
        const byteNumbers = new Array(byteCharacters.length).fill(0).map((_, i) =>
            byteCharacters.charCodeAt(i)
        );
        const byteArray = new Uint8Array(byteNumbers);
        const blob = new Blob([byteArray], { type: "application/pdf" });
        const blobUrl = URL.createObjectURL(blob);

        // Abrir en nueva ventana o pestaña
        const pdfWindow = window.open(blobUrl, "_blank");
        if (!pdfWindow) {
            ToastifyCustom({
                message: "No se pudo abrir la vista previa del PDF.",
                type: "error",
            });
        }
    } catch (error) {
        console.error("Error al mostrar PDF:", error);
        ToastifyCustom({
            message: "Hubo un problema al procesar el PDF.",
            type: "error",
        });
    }
};
