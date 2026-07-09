import Swal from "sweetalert2";
import { ToastifyCustom } from "../../util/ToastifyCustom ";
import { ApiResponse } from "../../interfaces/ApiResponse";

export const handleDeleteRoom = (
    roomId: number,
    roomName: string,
    onDelete: (idRoom: number) => Promise<ApiResponse<number>>
) => {
    Swal.fire({
        title: "Atención, acción irreversible",
        text: `Eliminar la sala "${roomName}" es una acción permanente y no se puede deshacer.`,
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Quiero eliminarla",
        cancelButtonText: "Cancelar",
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire({
                title: "Confirmación requerida",
                html: `Para confirmar la eliminación, escribe el nombre exacto de la sala: <strong>"${roomName}"</strong>`,
                input: "text",
                inputPlaceholder: "Nombre de la sala",
                showCancelButton: true,
                confirmButtonText: "Eliminar definitivamente",
                cancelButtonText: "Cancelar",
                inputValidator: (value) => {
                    if (value !== roomName) {
                        return "El nombre no coincide. Por favor, escribe el nombre exacto de la sala.";
                    }
                    return null;
                },
            }).then(async (result2) => {
                if (result2.isConfirmed) {
                    try {
                        const result = await onDelete(roomId);
                        if (result.result) {
                            ToastifyCustom({
                                options: { autoClose: 2000, position: "bottom-right" },
                                message: "La sala fue eliminada correctamente.",
                                type: "success",
                            });
                        } else {
                            ToastifyCustom({
                                message: result.message
                                    ? result.message[0]
                                    : "Ocurrió un error al eliminar",
                                type: "error",
                            });
                        }
                    } catch (error) {
                        console.error("Error al eliminar la sala:", error);
                        ToastifyCustom({
                            message: "Error inesperado al eliminar la sala",
                            type: "error",
                        });
                    }
                }
            });
        }
    });
};
