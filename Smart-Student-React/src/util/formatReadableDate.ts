export function formatReadableDate(dateString: string): string {
    const date = new Date(dateString);

    const options: Intl.DateTimeFormatOptions = {
        day: "2-digit",
        month: "long",
        year: "numeric",
        timeZone: "UTC",
    };

    return date.toLocaleDateString("es-ES", options);
}
