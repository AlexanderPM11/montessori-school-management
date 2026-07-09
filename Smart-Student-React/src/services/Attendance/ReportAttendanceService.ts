import { ApiResponse } from "../../interfaces/ApiResponse";

import BaseApi from "../BaseApi";

class ReportAttendanceService {
    static async GetPdf(
        idRoom: number,
        date: string
    ): Promise<ApiResponse<string>> {
        const params = {
            idRoom: idRoom,
            date: date,
        };
        try {
            const response = await BaseApi.getAsync(
                `/ReportAttendance/GetPdf`,
                params
            );
            const dataResponse = response.data;
            return {
                data: dataResponse.data,
                result: response.data.result,
                message: response.data.messages[0],
            };
        } catch (error) {
            console.log(error);
            return {
                data: {} as string,
                result: false,
                message: "Error al obtener los Subjects.",
            };
        }
    }
    static async GetPdfStudent(
        idRoom: number,
        date: string
    ): Promise<ApiResponse<string>> {
        const params = {
            idStudent: idRoom,
            date: date,
        };
        try {
            const response = await BaseApi.getAsync(
                `/ReportAttendance/GetPdfStudent`,
                params
            );
            const dataResponse = response.data;
            return {
                data: dataResponse.data,
                result: response.data.result,
                message: response.data.messages[0],
            };
        } catch (error) {
            console.log(error);
            return {
                data: {} as string,
                result: false,
                message: "Error al obtener los Subjects.",
            };
        }
    }
}

export default ReportAttendanceService;
