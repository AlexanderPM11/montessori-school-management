export function onlyNumbers(text: string): string {
    const targetResult = text.replace(/\D/g, ''); // Elimina todo lo que no sea un dígito
    return targetResult;
}

export function formatCardID(input: string, previousValue: string): string {
    const digits = onlyNumbers(input); // Filtra solo los números

    // Si el usuario está eliminando caracteres (input es más corto que previousValue), permitimos la edición
    if (input.length < previousValue.length) {
        // Si no hay dígitos, retornamos una cadena vacía
        if (digits.length === 0) {
            return '';
        }

        // Si hay menos de 3 dígitos, retornamos solo los dígitos
        if (digits.length <= 3) {
            return digits;
        }

        // Si hay entre 4 y 10 dígitos, retornamos el formato parcial
        if (digits.length <= 10) {
            return `${digits.slice(0, 3)}-${digits.slice(3)}`;
        }

        // Si hay 11 dígitos, retornamos el formato completo
        return `${digits.slice(0, 3)}-${digits.slice(3, 10)}-${digits[10]}`;
    }

    // Si el valor ya tiene el formato completo (XXX-XXXXXXX-X), no permitimos más cambios
    if (digits.length === 11) {
        return `${digits.slice(0, 3)}-${digits.slice(3, 10)}-${digits[10]}`;
    }

    // Si el valor tiene más de 11 dígitos, no permitimos más entradas
    if (digits.length > 11) {
        return previousValue;
    }

    // Si no tiene el formato completo, lo formateamos según el número de dígitos
    if (digits.length <= 3) {
        return digits;
    }

    if (digits.length <= 10) {
        return `${digits.slice(0, 3)}-${digits.slice(3)}`;
    }

    // Si tiene 11 dígitos, lo formateamos como XXX-XXXXXXX-X
    return `${digits.slice(0, 3)}-${digits.slice(3, 10)}-${digits[10]}`;
}

export function formatTel(input: string, previousValue: string): string {
    const digits = onlyNumbers(input); // Filtra solo los números

    // Si el usuario está eliminando caracteres (input es más corto que previousValue), permitimos la edición
    if (input.length < previousValue.length) {
        // Si no hay dígitos, retornamos una cadena vacía
        if (digits.length === 0) {
            return '';
        }

        // Si hay menos de 3 dígitos, retornamos solo los dígitos
        if (digits.length <= 3) {
            return digits;
        }

        // Si hay entre 4 y 6 dígitos, retornamos el formato parcial (809)-000
        if (digits.length <= 6) {
            return `(${digits.slice(0, 3)})-${digits.slice(3)}`;
        }

        // Si hay entre 7 y 10 dígitos, retornamos el formato (809)-000-0000
        return `(${digits.slice(0, 3)})-${digits.slice(3, 6)}-${digits.slice(6, 10)}`;
    }

    // Si el valor ya tiene el formato completo (809)-000-0000, no permitimos más cambios
    if (digits.length === 10) {
        return `(${digits.slice(0, 3)})-${digits.slice(3, 6)}-${digits.slice(6, 10)}`;
    }

    // Si el valor tiene más de 10 dígitos, no permitimos más entradas
    if (digits.length > 10) {
        return previousValue;
    }

    // Si no tiene el formato completo, lo formateamos según el número de dígitos
    if (digits.length <= 3) {
        return digits;
    }

    if (digits.length <= 6) {
        return `(${digits.slice(0, 3)})-${digits.slice(3)}`;
    }

    // Si tiene entre 7 y 10 dígitos, lo formateamos como (809)-000-0000
    return `(${digits.slice(0, 3)})-${digits.slice(3, 6)}-${digits.slice(6, 10)}`;
}