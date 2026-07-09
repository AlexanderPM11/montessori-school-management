// attendancePdfUtils.ts

import { ApiResponse } from "../../interfaces/ApiResponse";
import { AttendanceSummaryViewModel } from "../../interfaces/Attendace/AttendanceSummaryViewModel";
import { closeCustomLoading, showCustomLoading } from "../showCustomLoading";
import { ToastifyCustom } from "../ToastifyCustom ";

// Utilidad para obtener el nombre del mes en español
export function getMonthName(dateString: string): string {
    const date = new Date(dateString);
    return date.toLocaleString("es-ES", { month: "long" });
}

// Descargar PDF base64
export function downloadBase64Pdf(base64Data: string, fileName: string): void {
    const linkSource = `data:application/pdf;base64,${base64Data}`;
    const downloadLink = document.createElement("a");
    downloadLink.href = linkSource;
    downloadLink.download = fileName;
    downloadLink.click();
}

// Obtener el PDF general de asistencia de un salón
export async function handleGeneratePdf(
    idRoom: number,
    selectedDate: string,
    getPdf: (idRoom: number, selectedDate: string) => Promise<ApiResponse<string>>
): Promise<void> {
    try {
        showCustomLoading();
        const data = await getPdf(idRoom, selectedDate);

        if (data.result) {
            const dateObj = new Date(selectedDate);
            const monthName = dateObj.toLocaleString("es-ES", { month: "long" });

            const capitalizedMonth =
                monthName.charAt(0).toUpperCase() + monthName.slice(1);
            const fileName = `Registro_Asistencia_${capitalizedMonth}.pdf`;

            downloadBase64Pdf(data.data, fileName);
        } else {
            ToastifyCustom({
                message:
                    data.message ??
                    "Ocurrió un error al generar el PDF del registro de asistencia. Por favor, intente nuevamente o contacte al administrador del sistema.",
                type: "error",
            });
        }
    } catch (error) {
        console.error("Error fetching attendance data:", error);
    } finally {
        closeCustomLoading();
    }
}

// Obtener el PDF de asistencia individual de un estudiante
export async function getPdfStudent(
    idStudent: number,
    selectedDate: string,
    onGetPdfStudent: (
        idStudent: number,
        selectedDate: string
    ) => Promise<ApiResponse<string>>,
    attendacesInfo: AttendanceSummaryViewModel
): Promise<void> {
    try {
        showCustomLoading();
        const data = await onGetPdfStudent(idStudent, selectedDate);

        if (data.result) {
            const monthName = getMonthName(selectedDate);
            const monthNameCapitalized =
                monthName.charAt(0).toUpperCase() + monthName.slice(1);

            const student = attendacesInfo.attendances.find(
                (s) => s.idStudent === idStudent
            );

            const sanitizedStudentName =
                student?.fullNameStudent?.replace(/ /g, "_")?.trim() ?? "Estudiante";

            downloadBase64Pdf(
                data.data,
                `Asistencia_${sanitizedStudentName}_${monthNameCapitalized}.pdf`
            );
        } else {
            ToastifyCustom({
                message:
                    data.message ??
                    "Ocurrió un error al generar el PDF del registro de asistencia. Por favor, intente nuevamente o contacte al administrador del sistema.",
                type: "error",
            });
        }
    } catch (error) {
        console.error("Error fetching attendance data:", error);
    } finally {
        closeCustomLoading();
    }
}

// Obtener la data de asistencia
export async function fetchAttendanceData(
    idRoom: number,
    selectedDate: string,
    getAttendanceInfo: (
        idRoom: number,
        dayWeek: string
    ) => Promise<ApiResponse<AttendanceSummaryViewModel>>
): Promise<void> {
    try {
        showCustomLoading();
        await getAttendanceInfo(idRoom, selectedDate);
    } catch (error) {
        console.error("Error fetching attendance data:", error);
    } finally {
        closeCustomLoading();
    }
}
