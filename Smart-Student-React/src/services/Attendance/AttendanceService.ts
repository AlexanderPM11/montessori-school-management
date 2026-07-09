import { ApiResponse } from "../../interfaces/ApiResponse";
import {
    AttendanceSummaryViewModel,
    AttendanceSummaryViewModelConvert,
} from "../../interfaces/Attendace/AttendanceSummaryViewModel";
import { RequestAttendance } from "../../interfaces/Attendace/RequestAttendance";
import BaseApi from "../BaseApi";

class AttendaceService {
    static async GetAttendanceInfo(
        idRoom: number,
        dayWeek: string
    ): Promise<ApiResponse<AttendanceSummaryViewModel>> {
        const params = {
            idRoom: idRoom,
            dayWeek: dayWeek,
        };
        try {
            const response = await BaseApi.getAsync(
                `/Attendance/AttendanceInfo`,
                params
            );
            const dataResponse = response.data.data;

            const attendace: AttendanceSummaryViewModel =
                AttendanceSummaryViewModelConvert.toAttendanceSummaryViewModel(
                    dataResponse
                );
            return {
                data: attendace,
                result: response.data.result,
                message: response.data.messages[0],
            };
        } catch (error) {
            console.log(error);
            return {
                data: {} as AttendanceSummaryViewModel,
                result: false,
                message: "Error al obtener los Subjects.",
            };
        }
    }
    static async UpdateAttendance(
        request: RequestAttendance
    ): Promise<ApiResponse<string>> {
        try {
            const response = await BaseApi.postAsync(
                "/Attendance/UpdateAttendance",
                request
            );

            const dataResponse = response.data;

            return {
                data: dataResponse,
                result: response.data.result,
                message: response.data.messages[0],
            };
        } catch (error) {
            console.log(error);
            return {
                data: "",
                result: false,
                message: "Error al crear o actualizar el Subject.",
            };
        }
    }
    static async Delete(idAttendance: number): Promise<ApiResponse<number>> {
        try {
            const response = await BaseApi.deleteAsync(
                `/Attendance/DeleteAttendance/${idAttendance}`
            );

            return {
                data: response.data,
                result: true,
                message: "Registro eliminado con éxito!",
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

    static async MakeAllPresent(
        request: RequestAttendance
    ): Promise<ApiResponse<string>> {
        try {
            const response = await BaseApi.postAsync(
                "/Attendance/MakeAllPresent",
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
                data: "",
                result: false,
                message: "Error al crear o actualizar el Subject.",
            };
        }
    }
}

export default AttendaceService;
