export const downloadBase64Pdf = (base64: string, fileName: string = "documento.pdf") => {
    if (!base64) {
        alert("No se proporcionó contenido para descargar.");
        return;
    }

    const byteCharacters = atob(base64);
    const byteArray = new Uint8Array(
        Array.from(byteCharacters, (char) => char.charCodeAt(0))
    );
    const blob = new Blob([byteArray], { type: "application/pdf" });

    const link = document.createElement("a");
    link.href = URL.createObjectURL(blob);
    link.download = fileName;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};
