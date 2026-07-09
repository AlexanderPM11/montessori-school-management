import { ApiResponse } from "../../interfaces/ApiResponse";
import { FathersAndMothers } from "../../interfaces/Auth/FathersAndMothers";

import { User, UserConvert } from "../../interfaces/Auth/User";
import { QuitAddTeacherRoom } from "../../interfaces/Room/QuitAddTeacherRoom";
import BaseApi from "../BaseApi";

class UserService {
    static async GetAllUsersAsync(): Promise<ApiResponse<User[]>> {
        try {
            const response = await BaseApi.getAsync("/Account/GetAllUsers");
            const dataResponse = response.data.data;

            const userData: User[] = Array.isArray(dataResponse)
                ? dataResponse.map((user: object) => UserConvert.toUser(user))
                : [];

            return {
                data: userData,
                result: response.data.result,
                message: response.data.messages[0],
            };
        } catch (error) {
            console.log(error);
            return {
                data: {} as User[],
                result: false,
                message: "Error al obtener los usuarios.",
            };
        }
    }
    static async GetFathersAndMothers(): Promise<ApiResponse<FathersAndMothers>> {
        try {
            const response = await BaseApi.getAsync("/Account/GetFathersAndMothers");
            const dataResponse = response.data.data;
            const target: FathersAndMothers = {} as FathersAndMothers;

            if (response.data.result) {
                target.fathers = dataResponse.fathers;
                target.mothers = dataResponse.mothers;

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
                data: {} as FathersAndMothers,
                result: false,
                message: "Error al obtener los padres y madres.",
            };
        }
    }
    static async ActiveInactive(idUser: string): Promise<ApiResponse<string>> {
        try {
            const response = await BaseApi.postAsync(
                `/Account/ActiveInactive/${idUser}`
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
                message: "error al activar o desactivar el usuario.",
            };
        }
    }
    static async CreateOrUpdate(user: User): Promise<ApiResponse<string>> {
        try {
            const formData = this.GetFormDataUser(user);
            const response = await BaseApi.postAsync(
                "/Account/CreateOrUpdate",
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
                data: "",
                result: false,
                message: "Error al crear o actualizar el usuario.",
            };
        }
    }
    static async ImportUsers(file: File): Promise<ApiResponse<string>> {
        try {
            const formData = new FormData();
            formData.append("file", file);
            const response = await BaseApi.postAsync(
                "/Account/ImportUsers",
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
                data: "",
                result: false,
                message: "Error al importar los usuarios.",
            };
        }
    }
    static async GetAllRolesAsync(): Promise<ApiResponse<string[]>> {
        try {
            const response = await BaseApi.getAsync("/Account/GetAllRoles");
            const dataResponse = response.data.data;

            return {
                data: dataResponse,
                result: response.data.result,
                message: response.data.messages[0],
            };
        } catch (error) {
            console.log(error);
            return {
                data: {} as string[],
                result: false,
                message: "Error al obtener los roles.",
            };
        }
    }

    // Teacher Rooms
    static async GetTeacherToAddRoom(
        idRoom: number
    ): Promise<ApiResponse<User[]>> {
        try {
            const response = await BaseApi.getAsync(
                `/Account/GetTeacherToAddRoom/${idRoom}`
            );
            const dataResponse = response.data.data;

            const userData: User[] = Array.isArray(dataResponse)
                ? dataResponse.map((user: object) => UserConvert.toUser(user))
                : [];

            return {
                data: userData,
                result: response.data.result,
                message: response.data.messages[0],
            };
        } catch (error) {
            console.log(error);
            return {
                data: {} as User[],
                result: false,
                message: "Error al obtener los profesores para agregar al salón.",
            };
        }
    }
    static async GetTeacherRoom(idRoom: number): Promise<ApiResponse<User[]>> {
        try {
            const response = await BaseApi.getAsync(
                `/Account/GetTeacherRoom/${idRoom}`
            );
            const dataResponse = response.data.data;

            const userData: User[] = Array.isArray(dataResponse)
                ? dataResponse.map((user: object) => UserConvert.toUser(user))
                : [];

            return {
                data: userData,
                result: response.data.result,
                message: response.data.messages[0],
            };
        } catch (error) {
            console.log(error);
            return {
                data: {} as User[],
                result: false,
                message: "Error al obtener los profesores del salón.",
            };
        }
    }
    static async AddTeacherRoom(
        data: QuitAddTeacherRoom
    ): Promise<ApiResponse<string>> {
        try {
            const response = await BaseApi.postAsync("/Account/AddTeacherRoom", data);

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
                message: "Error al agregar el profesor al salón.",
            };
        }
    }
    static async QuitTeacherRoom(
        data: QuitAddTeacherRoom
    ): Promise<ApiResponse<string>> {
        try {
            const response = await BaseApi.postAsync("/Account/QuitTeacherRoom", data);

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
                message: "Error al quitar el profesor del salón.",
            };
        }
    }
    // Private Methods
    private static GetFormDataUser(user: User): FormData {
        // Crear una instancia de FormData
        const formData = new FormData();

        // Agregar los datos de usuario al FormData
        formData.append("id", user.id);
        formData.append("userName", user.userName);
        formData.append("email", user.email);
        formData.append("roles", JSON.stringify(user.roles));
        formData.append("isVerified", String(user.isVerified));
        formData.append("firstName", user.firstName);
        formData.append("lastName", user.lastName);
        formData.append("addres", user.addres);
        formData.append("dateBirth", user.dateBirth);
        formData.append("gender", String(user.gender));
        formData.append("password", user.password);
        formData.append("confirmPassword", user.confirmPassword);
        formData.append("phoneNumber", user.phoneNumber);
        formData.append("estado", String(user.estado));
        formData.append("idUserCreator", user.idUserCreator);
        formData.append("institutionId", user.institutionId ?? 0);
        formData.append("institutionIdPrincipal", user.institutionIdPrincipal ?? 0);
        formData.append("urlImage", user.urlImage);
        formData.append("statu", String(user.statu));
        formData.append("tel", user.tel);
        formData.append("profession", user.profession);
        formData.append("occupation", user.occupation);
        formData.append("job", user.job);
        formData.append("placeWork", user.placeWork);
        formData.append("identificationId", user.identificationId);
        formData.append("idNivelEducativo", user.idNivelEducativo);
        formData.append("titleAchieved", user.titleAchieved);
        formData.append("studiesCurrentlyPursuing", user.studiesCurrentlyPursuing);
        formData.append("civilStatus", user.civilStatus);
        formData.append("nationality", user.nationality);
        formData.append(
            "yearsServiceEducationalSystem",
            user.yearsServiceEducationalSystem ?? 0
        );
        formData.append("yearServiceGrade", user.yearServiceGrade ?? 0);
        formData.append("areaSpecialization", user.areaSpecialization ?? 0);
        formData.append(
            "worksAnActivityDiferentThanteaching",
            String(user.worksAnActivityDiferentThanteaching)
        );
        formData.append("specify", user.specify);
        formData.append("relationshipId", user.relationshipId);
        formData.append("noBook", user.noBook);
        formData.append("noFolio", user.noFolio);
        formData.append("token", user.token);
        formData.append("refreshToken", user.refreshToken);

        if (user.file) {
            formData.append("file", user.file);
        }

        return formData;
    }
}

export default UserService;
