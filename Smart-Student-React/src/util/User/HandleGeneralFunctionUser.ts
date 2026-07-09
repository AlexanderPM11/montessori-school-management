import { ApiResponse } from "../../interfaces/ApiResponse";
import ManageLocalStorage from "../manageLocalStorage";



interface UploadHandlerOptions {
    file: File;
    onProgress: (progress: number, error?: string) => void;
    onImportUsers: (file: File) => Promise<ApiResponse<string>>;
    localStorageKey: string;
}
export const handleGeneralFunctionUser = async ({ file, onProgress, onImportUsers, localStorageKey }: UploadHandlerOptions) => {
    try {
        onProgress(0);

        // Progreso inicial
        for (let i = 0; i <= 30; i += 5) {
            await new Promise((resolve) => setTimeout(resolve, 200));
            onProgress(i);
        }

        const res = await onImportUsers(file);

        if (!res?.result) {
            onProgress(0, res.message || "Error al procesar el archivo");
            return;
        }
        // Progreso final
        for (let i = 40; i <= 100; i += 5) {
            await new Promise((resolve) => setTimeout(resolve, 300));
            onProgress(i);
        }
        ManageLocalStorage.saveToLocalStorage(
            localStorageKey,
            true
        );
    } catch (error) {
        onProgress(
            0,
            error instanceof Error ? error.message : "Error desconocido"
        );
    }
};