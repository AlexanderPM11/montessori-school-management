import { ApiResponse } from "../../interfaces/ApiResponse";
import { Room, RoomConvert } from "../../interfaces/Room/Room";
import BaseApi from "../BaseApi";

class RoomService {
    static async GetAllRooms(): Promise<ApiResponse<Room[]>> {
        try {
            const response = await BaseApi.getAsync("/Room/GetAllRooms");
            const dataResponse = response.data.data;

            const roomData: Room[] = Array.isArray(dataResponse)
                ? dataResponse.map((room: object) => RoomConvert.toRoom(room))
                : [];

            if (response.data.result) {
                return {
                    data: roomData,
                    result: response.data.result,
                    message: response.data.messages[0],
                };
            }

            return {
                data: [] as Room[],
                result: false,
                message: response.data.messages
                    ? response.data.messages[0]
                    : "No message",
            };
        } catch (error) {
            console.log(error);
            return {
                data: [] as Room[],
                result: false,
                message: "Error al obtener los salones.",
            };
        }
    }
    static async CreateOrUpdate(room: Room): Promise<ApiResponse<number>> {
        try {
            const formData = this.GetFormDataRoom(room);
            const response = await BaseApi.postAsync(
                "/Room/CreateOrUpdate",
                formData,
                {
                    headers: {
                        "Content-Type": "multipart/form-data",
                    },
                }
            );

            const dataResponse = response.data.data;
            console.log(dataResponse)

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
            const response = await BaseApi.deleteAsync(`/Room/Delete/${idRoom}`);
            return {
                data: response.data,
                result: response.data.result,
                message: response.data.messages,
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

    // Private Methods
    private static GetFormDataRoom(room: Room): FormData {
        const formData = new FormData();

        try {
            formData.append("id", room.id.toString() || "");
            formData.append("name", room.name || "");
            formData.append("description", room.description || "");
            formData.append("idTeacherLead", room.idTeacherLead?.toString() || "");
            formData.append("location", room.location?.toString() || "");
            formData.append("capacity", room.capacity?.toString() || "");
            formData.append("idTypeRegisters", room.level?.toString() || "");

            if (room.file) {
                formData.append("file", room.file);
            }
        } catch (error) {
            console.error("Error al crear el GetFormDataRoom:", error);
            return new FormData(); // Retorna un FormData vacío en caso de error
        }
        return formData;
    }
}

export default RoomService;
