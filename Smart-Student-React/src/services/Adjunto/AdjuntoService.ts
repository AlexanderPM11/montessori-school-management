
import { Adjunto, AdjuntoConvert } from "../../interfaces/Adjunto/Ajunto";
import { ApiResponse } from "../../interfaces/ApiResponse";
import BaseApi from "../BaseApi";


class AdjuntoService {

    static async GetAdjunto(idStudent: number): Promise<ApiResponse<Adjunto[]>> {

        try {

            const response = await BaseApi.getAsync(`/Adjunto/GetAdjunto/${idStudent}`);
            const dataResponse = response.data.data;

            const adjuntoData: Adjunto[] = Array.isArray(dataResponse)
                ? dataResponse.map((adjunto: object) => AdjuntoConvert.toAjunto(adjunto))
                : [];

            return {
                data: adjuntoData,
                result: response.data.result,
                message: response.data.messages[0]
            }

        } catch (error) {
            console.log(error)
            return {
                data: {} as Adjunto[],
                result: false,
                message: "Error al obtener los adjuntos."
            }
        }
    }
    static async CreateOrUpdate(adjunto: Adjunto): Promise<ApiResponse<number>> {

        try {

            const formData = this.GetFormDataAdjunto(adjunto);
            const response = await BaseApi.postAsync('/Adjunto/CreateOrUpdate', formData, {
                headers: {
                    'Content-Type': 'multipart/form-data',
                }
            });

            const dataResponse = response.data.data;

            return {
                data: dataResponse,
                result: response.data.result,
                message: response.data.messages[0]
            };

        } catch (error) {
            console.log(error);
            return {
                data: 0,
                result: false,
                message: 'Error al crear o actualizar el adjunto.'
            };
        }
    }
    static async Delete(idAdjunto: number): Promise<ApiResponse<number>> {

        try {

            const response = await BaseApi.deleteAsync(`/Adjunto/Delete/${idAdjunto}`);

            return {
                data: response.data,
                result: true,
                message: 'Registro eliminado con éxito!'
            };


        } catch (error) {
            console.log(error)
            return {
                data: 0,
                result: false,
                message: "Error al eliminar el adjunto."
            }
        }
    }

    // Private Methods
    private static GetFormDataAdjunto(adjunto: Adjunto): FormData {
        const formData = new FormData();
        try {

            formData.append("id", adjunto.id.toString() || "0");
            formData.append("name", adjunto.name || "");
            formData.append("description", adjunto.description || "");
            formData.append("path", adjunto.path || "");
            formData.append("idStudent", adjunto.idStudent.toString() || "0");
            formData.append("idTipoAdjunto", adjunto.idTipoAdjunto.toString() || "0");


            if (adjunto.file) {
                formData.append("archivo", adjunto.file);
            }
        } catch (error) {
            console.error("Error al crear los datos GetFormDataAdjunto", error);
            return new FormData(); // Retorna un FormData vacío en caso de error
        }
        return formData;
    }


}

export default AdjuntoService;



