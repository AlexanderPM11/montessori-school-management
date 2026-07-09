import { ApiResponse } from "../../interfaces/ApiResponse";
import { EducationalInstituConvert, EducationalInstitutionViewModel } from "../../interfaces/IntiituCenter/EducationalInstitutionViewModel";
import BaseApi from "../BaseApi";

class InstituCenterService {
    static async GetInstituCenter(): Promise<ApiResponse<EducationalInstitutionViewModel>> {
        try {
            const response = await BaseApi.getAsync("/EducationalInstitu/GetInstituCenter");
            const dataResponse = response.data.data;

            const centerData: EducationalInstitutionViewModel = EducationalInstituConvert.toEducationalInstitu(dataResponse);
            if (response.data.result) {
                return {
                    data: centerData,
                    result: response.data.result,
                    message: response.data.messages[0],
                };
            }

            return {
                data: {} as EducationalInstitutionViewModel,
                result: false,
                message: response.data.messages
                    ? response.data.messages[0]
                    : "No message",
            };
        } catch (error) {
            console.log(error);
            return {
                data: {} as EducationalInstitutionViewModel,
                result: false,
                message: "Error al obtener el centro.",
            };
        }
    }
    static async CreateOrUpdate(room: EducationalInstitutionViewModel): Promise<ApiResponse<number>> {
        try {
            const formData = this.GetFormDataInstitud(room);
            const response = await BaseApi.postAsync(
                "/EducationalInstitu/CreateUpdate",
                formData,
                {
                    headers: {
                        "Content-Type": "multipart/form-data",
                    },
                }
            );

            const dataResponse = response.data.data;

            return {
                data: dataResponse,
                result: response.data.result,
                message: response.data.messages[0],
            };
        } catch (error) {
            console.log(error);
            return {
                data: 0,
                result: false,
                message: "Error al crear o actualizar el centro.",
            };
        }
    }

    // Private Methods
    private static GetFormDataInstitud(room: EducationalInstitutionViewModel): FormData {
        const formData = new FormData();

        try {
            if (room.id) formData.append("id", room.id);
            formData.append("name", room.name);
            formData.append("address", room.address);
            formData.append("nameMunicipality", room.nameMunicipality);
            if (room.phone) formData.append("phone", room.phone);
            formData.append("mobile", room.mobile);
            formData.append("idRector", room.idRector);
            formData.append("idCordinator", room.idCordinator);
            if (room.idProvinceDom !== undefined) formData.append("idProvinceDom", room.idProvinceDom.toString());
            if (room.idDepartment !== undefined) formData.append("idDepartment", room.idDepartment.toString());
            if (room.idSecretary) formData.append("idSecretary", room.idSecretary);
            if (room.academicResolution) formData.append("academicResolution", room.academicResolution);
            if (room.educationalRegistry) formData.append("educationalRegistry", room.educationalRegistry);
            if (room.website) formData.append("website", room.website);
            if (room.urlLogo) formData.append("urlLogo", room.urlLogo);
            formData.append("isMainSchool", room.isMainSchool.toString());
            if (room.idUser) formData.append("idUser", room.idUser);
            formData.append("userAssignmentId", room.userAssignmentId);
            formData.append("session", room.session);
            formData.append("regional", room.regional);
            formData.append("district", room.district);

            if (room.file) {
                formData.append("file", room.file);
            }
        } catch (error) {
            console.error("Error al crear el FormData de Institución:", error);
            return new FormData(); // Retorna un FormData vacío en caso de error
        }

        return formData;
    }


}

export default InstituCenterService;
