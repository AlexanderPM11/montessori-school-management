import Swal from "sweetalert2";
import { ToastifyCustom } from "../../../util/ToastifyCustom ";
import { RequestAttendance } from "../../../interfaces/Attendace/RequestAttendance";

export interface MarkAllPresentRequest {
    idRoom: number;
    dayWeek: string; // Puedes usar Date si tu API lo soporta, y formatearlo con dayjs, etc.
    idStudent: number; // 0 para aplicar a todos
    status: string; // si usas estados como "PRESENT" o "" para marcar
}
export const handleMarkAllPresent = async (
    markAllAsPresent: (request: RequestAttendance) => void,
    { idRoom, dayWeek, idStudent, status }: MarkAllPresentRequest
): Promise<void> => {
    try {
        const firstConfirm = await Swal.fire({
            title: "Atención, acción masiva",
            text: `Se marcarán TODOS los estudiantes de la sala como PRESENTES para la fecha seleccionada.`,
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Sí, continuar",
            cancelButtonText: "Cancelar",
        });

        if (!firstConfirm.isConfirmed) return;

        const request = {
            idRoom: idRoom,
            dayWeek: dayWeek,
            idStudent: idStudent,
            status: status,
        };
        markAllAsPresent(request);
    } catch (error) {
        console.error("Error al marcar asistencia masiva:", error);
        ToastifyCustom({
            message: "Error inesperado al procesar la solicitud.",
            type: "error",
        });
    }
};
