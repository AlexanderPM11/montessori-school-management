import { ApiResponse } from "../../interfaces/ApiResponse";
import { GendersCivStatuNacLevEducRelati } from "../../interfaces/General/GendersCivStatuNacLevEducRelati";
import { GeneralUserRoles } from "../../interfaces/General/GeneralUserRoles";
import { SelectedInterface } from "../../interfaces/SelectedInterface";
import BaseApi from "../BaseApi";

class GeneralService {
    static async GetGendersCivStatuNacLevEducRelati(): Promise<
        ApiResponse<GendersCivStatuNacLevEducRelati>
    > {
        try {
            const response = await BaseApi.getAsync(
                "/GeneralData/GetGendersCivStatuNacLevEducRelati"
            );
            const dataResponse = response.data.data;

            const target: GendersCivStatuNacLevEducRelati =
                {} as GendersCivStatuNacLevEducRelati;
            if (response.data.result) {
                target.civilStatus = dataResponse.civilStatus;
                target.educationalLevel = dataResponse.educationalLevel;
                target.nationality = dataResponse.nationality;
                target.relationship = dataResponse.relationship;
                target.professions = dataResponse.professions;
                target.grades = dataResponse.grades;
                target.levels = dataResponse.levels;
                target.genders = [
                    {
                        id: 0,
                        text: "Femenino",
                    },
                    {
                        id: 1,
                        text: "Masculino",
                    },
                ];
                return {
                    data: target,
                    result: response.data.result,
                    message: response.data.messages[0],
                };
            }

            return {
                data: target,
                result: false,
                message: response.data.messages[0],
            };
        } catch (error) {
            console.log(error);
            return {
                data: {} as GendersCivStatuNacLevEducRelati,
                result: false,
                message: "Error al obtener los datos generales.",
            };
        }
    }
    static async GetTeachers(): Promise<
        ApiResponse<SelectedInterface<string>[]>
    > {
        try {
            const response = await BaseApi.getAsync("/GeneralData/GetTeachers");
            const dataResponse = response.data.data;

            if (response.data.result) {
                return {
                    data: dataResponse,
                    result: response.data.result,
                    message: response.data.messages[0],
                };
            }

            return {
                data: [] as SelectedInterface<string>[],
                result: false,
                message: response.data.messages
                    ? response.data.messages[0]
                    : "No message",
            };
        } catch (error) {
            console.log(error);
            return {
                data: [] as SelectedInterface<string>[],
                result: false,
                message: "Error al obtener los profesores.",
            };
        }
    }
    static async GetGeneralsUserRoles(): Promise<ApiResponse<GeneralUserRoles>> {
        try {
            const response = await BaseApi.getAsync(
                "/GeneralData/GetGeneralsUserRoles"
            );
            const dataResponse = response.data.data;

            if (response.data.result) {
                return {
                    data: dataResponse as GeneralUserRoles,
                    result: response.data.result,
                    message: response.data.messages[0],
                };
            }

            return {
                data: {} as GeneralUserRoles,
                result: false,
                message: response.data.messages
                    ? response.data.messages[0]
                    : "No message",
            };
        } catch (error) {
            console.log(error);
            return {
                data: {} as GeneralUserRoles,
                result: false,
                message: "Error al obtener los usuarios por rol.",
            };
        }
    }

    static async GetTeachersByIdRoom(
        idRoom: number
    ): Promise<ApiResponse<SelectedInterface<string>[]>> {
        try {
            const response = await BaseApi.getAsync(
                `/GeneralData/GetTeachersByIdRoom/${idRoom}`
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
                data: [] as SelectedInterface<string>[],
                result: false,
                message: response.data.messages
                    ? response.data.messages[0]
                    : "No message",
            };
        } catch (error) {
            console.log(error);
            return {
                data: [] as SelectedInterface<string>[],
                result: false,
                message: "Error al obtener los profesores.",
            };
        }
    }
    static async GetLevels(): Promise<ApiResponse<SelectedInterface<number>[]>> {
        try {
            const response = await BaseApi.getAsync("/GeneralData/GetLevels");
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
                message: "Error al obtener los niveles.",
            };
        }
    }
    static async GetProvices(): Promise<ApiResponse<SelectedInterface<number>[]>> {
        try {
            const response = await BaseApi.getAsync("/GeneralData/GetProvices");
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
                message: "Error al obtener los niveles.",
            };
        }
    }
    static async GetPeriods(
        isPrimaria: boolean = false
    ): Promise<ApiResponse<SelectedInterface<string>[]>> {
        try {
            const response = await BaseApi.getAsync(
                `/GeneralData/GetPeriods/${isPrimaria}`
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
                data: [] as SelectedInterface<string>[],
                result: false,
                message: response.data.messages
                    ? response.data.messages[0]
                    : "No message",
            };
        } catch (error) {
            console.log(error);
            return {
                data: [] as SelectedInterface<string>[],
                result: false,
                message: "Error al obtener los niveles.",
            };
        }
    }
    static async GetDaysOfWeek(
    ): Promise<ApiResponse<string[]>> {
        try {
            const response = await BaseApi.getAsync(
                `/GeneralData/GetDaysOfWeek`
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
                data: [] as string[],
                result: false,
                message: response.data.messages
                    ? response.data.messages[0]
                    : "No message",
            };
        } catch (error) {
            console.log(error);
            return {
                data: [] as string[],
                result: false,
                message: "Error al obtener los niveles.",
            };
        }
    }
}

export default GeneralService;
