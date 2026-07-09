import { ApiResponse } from "../../interfaces/ApiResponse";
import { Subject } from "../../interfaces/Subject/Subject";
import { closeCustomLoading, showCustomLoading } from "../../util/showCustomLoading";
import { SweetAlertCustom } from "../../util/SweetAlerCustom";
import { ToastifyCustom } from "../../util/ToastifyCustom ";

export const handleInactiveActive = (
    idsubject: number,
    onDelete: (idRoom: number) => Promise<ApiResponse<number>>,
    subject: Subject
) => {
    SweetAlertCustom({
        title: "¿Estás seguro?",
        description: `¿Deseas eliminar esta Asignatura: ${subject.name}?`,
        type: "warning",
        showCancelButton: true,
        confirmButtonText: "Sí, continuar",
        cancelButtonText: "Cancelar",
        onConfirm: async () => {
            showCustomLoading();
            onDelete(idsubject).then((result) => {
                if (result.result) {
                    ToastifyCustom({
                        options: { autoClose: 2000, position: "bottom-right" },
                        message: `La Asignatura: ${subject.name} fue eliminada correctamente.`,
                        type: "success",
                    });
                } else {
                    ToastifyCustom({
                        message: result.message ? result.message[0] : "An error occurred",
                        type: "error",
                    });
                }
            });
            closeCustomLoading();
        },
    });
};
