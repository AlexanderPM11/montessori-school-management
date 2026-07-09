export default class ManageLocalStorage {
    // Guardar datos en localStorage
    static saveToLocalStorage(key: string, data: unknown): void {
        try {
            const serializedData = JSON.stringify(data); // Convierte el dato a string
            localStorage.setItem(key, serializedData); // Guarda en localStorage
        } catch (error) {
            console.error("Error saving to localStorage:", error);
        }
    }

    // Leer datos de localStorage
    static readFromLocalStorage<T>(key: string): T | null {
        try {
            const serializedData = localStorage.getItem(key); // Obtiene el dato
            if (serializedData === null) {
                return null; // Si no existe, retorna null
            }
            return JSON.parse(serializedData) as T; // Convierte el string a objeto
        } catch (error) {
            console.error("Error reading from localStorage:", error);
            return null;
        }
    }

    // Eliminar datos de localStorage
    static removeFromLocalStorage(key: string): void {
        try {
            localStorage.removeItem(key); // Elimina el dato
        } catch (error) {
            console.error("Error removing from localStorage:", error);
        }
    }
}