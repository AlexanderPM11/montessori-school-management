
import { ApiResponse } from "../../interfaces/ApiResponse";
import { Subject, SubjectConvert } from "../../interfaces/Subject/Subject";
import BaseApi from "../BaseApi";


class SubjectService {

    static async GetSubject(): Promise<ApiResponse<Subject[]>> {

        try {

            const response = await BaseApi.getAsync(`/Subject/GetAllSubject`);
            const dataResponse = response.data.data;

            const SubjectData: Subject[] = Array.isArray(dataResponse)
                ? dataResponse.map((Subject: object) => SubjectConvert.toAjunto(Subject))
                : [];

            return {
                data: SubjectData,
                result: response.data.result,
                message: response.data.messages[0]
            }

        } catch (error) {
            console.log(error)
            return {
                data: {} as Subject[],
                result: false,
                message: "Error al obtener los Subjects."
            }
        }
    }
    static async CreateOrUpdate(subject: Subject): Promise<ApiResponse<number>> {

        try {

            const formData = this.GetFormDataSubject(subject);
            const response = await BaseApi.postAsync('/Subject/CreateOrUpdate', formData, {
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
                message: 'Error al crear o actualizar el Subject.'
            };
        }
    }
    static async Delete(idSubject: number): Promise<ApiResponse<number>> {

        try {

            const response = await BaseApi.deleteAsync(`/Subject/Delete/${idSubject}`);

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
                message: "Error al eliminar el Subject."
            }
        }
    }

    // Private Methods
    private static GetFormDataSubject(subject: Subject): FormData {
        const formData = new FormData();
        try {
            formData.append("id", subject.id.toString() || "0");
            formData.append("name", subject.name || "");
            formData.append("description", subject.description || "");
            formData.append("code", subject.code || "");
            formData.append("imageUrl", subject.imageUrl || "");

            if (subject.file) {
                formData.append("file", subject.file);
            }
        } catch (error) {
            console.error("Error al crear los datos GetFormDataSubject", error);
            return new FormData(); // Retorna un FormData vacío en caso de error
        }
        return formData;
    }


}

export default SubjectService;



