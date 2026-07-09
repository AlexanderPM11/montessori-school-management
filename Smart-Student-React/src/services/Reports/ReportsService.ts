import { ApiResponse } from "../../interfaces/ApiResponse";
import {
    ReportDataResponse,
    ReportDataResponseConvert,
} from "../../interfaces/Reports/ReportDataResponse";
import { RequestReport } from "../../interfaces/Reports/RequestReporT";
import { RequestTeacherSubject } from "../../interfaces/Reports/RequestTeacherSubject";
import { SaveCommentsRequest } from "../../interfaces/Reports/SaveCommentsRequest";
import BaseApi from "../BaseApi";
import { SelectedInterface } from "../../interfaces/SelectedInterface";

class ReportsService {
    static async GetDataReport(
        requestReport: RequestReport
    ): Promise<ApiResponse<ReportDataResponse>> {
        try {
            const params = {
                idStudent: requestReport.idStudent,
                idAchievementIndicator: requestReport.idAchievementIndicator,
                period: requestReport.period,
                estado: requestReport.estado,
                idSubject: requestReport.idSubject,
            };
            const response = await BaseApi.getAsync("/Reports/GetDataReport", params);
            const dataResponse = response.data.data;

            const reportsData =
                ReportDataResponseConvert.toRequestReport(dataResponse);

            if (response.data.result) {
                return {
                    data: reportsData,
                    result: response.data.result,
                    message: response.data.messages[0],
                };
            }

            return {
                data: reportsData as ReportDataResponse,
                result: false,
                message: response.data.messages
                    ? response.data.messages[0]
                    : "No message",
            };
        } catch (error) {
            console.log(error);
            return {
                data: {} as ReportDataResponse,
                result: false,
                message: "Error al obtener los salones.",
            };
        }
    }
    static async GetReport(
        requestReport: RequestReport
    ): Promise<ApiResponse<ReportDataResponse>> {
        try {
            const params = {
                idStudent: requestReport.idStudent,
                idAchievementIndicator: requestReport.idAchievementIndicator,
                period: requestReport.period,
                estado: requestReport.estado,
                idSubject: requestReport.idSubject,
                preview: requestReport.preview,
            };
            const response = await BaseApi.getAsync("/Reports/GetReportPDF", params);
            const dataResponse = response.data.data;

            const reportsData =
                ReportDataResponseConvert.toRequestReport(dataResponse);

            if (response.data.result) {
                return {
                    data: reportsData,
                    result: response.data.result,
                    message: response.data.messages[0],
                };
            }

            return {
                data: reportsData as ReportDataResponse,
                result: false,
                message: response.data.messages
                    ? response.data.messages[0]
                    : "No message",
            };
        } catch (error) {
            console.log(error);
            return {
                data: {} as ReportDataResponse,
                result: false,
                message: "Error al obtener los salones.",
            };
        }
    }
    static async GetSubjectsByTeacher(
        requestReport: RequestTeacherSubject
    ): Promise<ApiResponse<SelectedInterface<number>[]>> {
        try {
            const params = {
                idTeacher: requestReport.idTeacher,
                idRoom: requestReport.idRoom,
            };
            const response = await BaseApi.getAsync(
                "/Reports/GetSubjectsByTeacher",
                params
            );
            const dataResponse = response.data.data;
            if (response.data.result) {
                return {
                    data: dataResponse,
                    result: response.data.result,
                    message: response.data.messages[0],
                };
            }
            return {
                data: [] as SelectedInterface<number>[],
                result: false,
                message: response.data.messages
                    ? response.data.messages[0]
                    : "No message",
            };
        } catch (error) {
            console.log(error);
            return {
                data: [] as SelectedInterface<number>[],
                result: false,
                message: "Error al obtener las materias del profesor.",
            };
        }
    }
    static async UpdateEvaluation(
        requestReport: RequestReport
    ): Promise<ApiResponse<number>> {
        try {
            const response = await BaseApi.postAsync(
                "/Reports/UpdateEvaluation",
                requestReport
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
                message: "Error al crear o actualizar el salón.",
            };
        }
    }
    static async UpdateCalification(
        requestReport: RequestReport
    ): Promise<ApiResponse<number>> {
        try {
            const response = await BaseApi.postAsync(
                "/Reports/UpdateCalification",
                requestReport
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
                message: "Error al crear o actualizar el salón.",
            };
        }
    }
    static async DeleteCalification(
        requestReport: RequestReport
    ): Promise<ApiResponse<number>> {
        try {
            const params = {
                idStudent: requestReport.idStudent,
                idAchievementIndicator: requestReport.idAchievementIndicator,
                period: requestReport.period,
                estado: requestReport.estado,
                selectedGradeId: requestReport.idSubject,
            };
            const response = await BaseApi.deleteAsync(
                `/Reports/DeleteCalification`,
                params
            );
            const dataResponse = response.data;
            return {
                data: requestReport.idAchievementIndicator,
                result: true,
                message: dataResponse.messages[0],
            };
        } catch (error) {
            console.log(error);
            return {
                data: 0,
                result: false,
                message: "Error al eliminar el salón.",
            };
        }
    }
    static async SaveComments(
        request: SaveCommentsRequest
    ): Promise<ApiResponse<number>> {
        try {
            const response = await BaseApi.postAsync(
                "/Reports/SaveComments",
                request
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
                message: "Error al crear o actualizar el salón.",
            };
        }
    }
    static async Delete(
        requestReport: RequestReport
    ): Promise<ApiResponse<number>> {
        try {
            const params = {
                idStudent: requestReport.idStudent,
                idAchievementIndicator: requestReport.idAchievementIndicator,
                period: requestReport.period,
                estado: requestReport.estado,
                idSubject: requestReport.idSubject,
            };
            const response = await BaseApi.deleteAsync(
                `/Reports/DeleteEvaluation`,
                params
            );
            const dataResponse = response.data;
            return {
                data: requestReport.idAchievementIndicator,
                result: true,
                message: dataResponse.messages[0],
            };
        } catch (error) {
            console.log(error);
            return {
                data: 0,
                result: false,
                message: "Error al eliminar el salón.",
            };
        }
    }
}

export default ReportsService;
