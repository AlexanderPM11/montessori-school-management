import { ApiResponse } from "../../interfaces/ApiResponse";
import {
    RoomSubject,
    RoomSubjectConvert,
} from "../../interfaces/Room/RoomSubject";
import BaseApi from "../BaseApi";

class RoomSubjectService {
    static async GetAllRoomTeacher(
        idRoom: number
    ): Promise<ApiResponse<RoomSubject[]>> {
        try {
            const response = await BaseApi.getAsync(
                `/RoomTeacher/GetAllRoomTeacher/${idRoom}`
            );
            const dataResponse = response.data.data;

            const roomData: RoomSubject[] = Array.isArray(dataResponse)
                ? dataResponse.map((room: object) =>
                    RoomSubjectConvert.toRoomSubject(room)
                )
                : [];
            if (response.data.result) {
                return {
                    data: roomData,
                    result: response.data.result,
                    message: response.data.messages[0],
                };
            }

            return {
                data: [] as RoomSubject[],
                result: false,
                message: response.data.messages
                    ? response.data.messages[0]
                    : "No message",
            };
        } catch (error) {
            console.log(error);
            return {
                data: [] as RoomSubject[],
                result: false,
                message: "Error al obtener los salones del profesor.",
            };
        }
    }
    static async CreateOrUpdate(room: RoomSubject): Promise<ApiResponse<number>> {
        try {
            const response = await BaseApi.postAsync(
                "/RoomTeacher/CreateOrUpdate",
                room,
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
                message: "Error al crear o actualizar el salón.",
            };
        }
    }

    static async Delete(idRoom: number): Promise<ApiResponse<number>> {
        try {
            const response = await BaseApi.deleteAsync(`/RoomTeacher/Delete/${idRoom}`);
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
}

export default RoomSubjectService;
