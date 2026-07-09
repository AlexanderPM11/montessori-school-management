
import { ApiResponse } from "../../interfaces/ApiResponse";
import { QuitAddStudentRoom } from "../../interfaces/Room/QuitAddStudentRoom";

import { Student, StudentConvert } from "../../interfaces/Students/Student";
import BaseApi from "../BaseApi";


class StudentService {

    static async GetAllStudentsAsync(): Promise<ApiResponse<Student[]>> {

        try {

            const response = await BaseApi.getAsync("/Students/GetAllStudents");
            const dataResponse = response.data.data;

            const studentData: Student[] = Array.isArray(dataResponse)
                ? dataResponse.map((student: object) => StudentConvert.toStudent(student))
                : [];

            return {
                data: studentData,
                result: response.data.result,
                message: response.data.messages[0]
            }

        } catch (error) {
            console.log(error)
            return {
                data: {} as Student[],
                result: false,
                message: "Error al obtener los estudiantes."
            }
        }
    }
    static async GetDatosEstudianteByParents(idParent: string): Promise<ApiResponse<Student[]>> {

        try {

            const response = await BaseApi.getAsync(`/Students/GetDatosEstudianteByParents/${idParent}`);
            const dataResponse = response.data.data;

            const studentData: Student[] = Array.isArray(dataResponse)
                ? dataResponse.map((student: object) => StudentConvert.toStudent(student))
                : [];

            return {
                data: studentData,
                result: response.data.result,
                message: response.data.messages[0]
            }

        } catch (error) {
            console.log(error)
            return {
                data: {} as Student[],
                result: false,
                message: "Error al obtener los estudiantes por padres."
            }
        }
    }
    static async ImportUsers(file: File): Promise<ApiResponse<string>> {

        try {
            const formData = new FormData();
            formData.append('file', file);
            const response = await BaseApi.postAsync('/Students/ImportStudent', formData, {
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
                data: '',
                result: false,
                message: 'Error al importar los estudiantes.'
            };
        }
    }
    static async CreateOrUpdate(student: Student): Promise<ApiResponse<string>> {

        try {

            const formData = this.GetFormDataUser(student);
            const response = await BaseApi.postAsync('/Students/CreateOrUpdate', formData, {
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
                data: '',
                result: false,
                message: 'Error al crear o actualizar el estudiante.'
            };
        }
    }
    static async ActiveInactive(idStudent: number): Promise<ApiResponse<string>> {

        try {

            const response = await BaseApi.postAsync(`/Students/ActiveInactiveStudent/${idStudent}`);
            const dataResponse = response.data.data;

            return {
                data: dataResponse,
                result: response.data.result,
                message: response.data.messages[0]
            }

        } catch (error) {
            console.log(error)
            return {
                data: '',
                result: false,
                message: "Error al activar o desactivar el estudiante."
            }
        }
    }

    // Student Rooms
    static async GetStudentsRoom(idRoom: number): Promise<ApiResponse<Student[]>> {

        try {

            const response = await BaseApi.getAsync(`/Students/GetStudentsRoom/${idRoom}`);
            const dataResponse = response.data.data;
            const studentData: Student[] = Array.isArray(dataResponse)
                ? dataResponse.map((student: object) => StudentConvert.toStudent(student))
                : [];

            return {
                data: studentData,
                result: response.data.result,
                message: response.data.messages[0]
            }

        } catch (error) {
            console.log(error)
            return {
                data: {} as Student[],
                result: false,
                message: "Error al obtener los estudiantes por padres."
            }
        }
    }
    static async GetStudentsToAddRoom(idRoom: number): Promise<ApiResponse<Student[]>> {

        try {

            const response = await BaseApi.getAsync(`/Students/GetStudentsToAddRoom/${idRoom}`);
            const dataResponse = response.data.data;
            const studentData: Student[] = Array.isArray(dataResponse)
                ? dataResponse.map((student: object) => StudentConvert.toStudent(student))
                : [];

            return {
                data: studentData,
                result: response.data.result,
                message: response.data.messages[0]
            }

        } catch (error) {
            console.log(error)
            return {
                data: {} as Student[],
                result: false,
                message: "Error al obtener los estudiantes por padres."
            }
        }
    }
    static async AddStudentRoom(
        data: QuitAddStudentRoom
    ): Promise<ApiResponse<number>> {
        try {
            const response = await BaseApi.postAsync("/Students/AddStudentRoom", data);

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
                message: "Error al agregar el profesor al salón.",
            };
        }
    }
    static async QuitStudentRoom(idStudent: number): Promise<ApiResponse<number>> {

        try {

            const response = await BaseApi.postAsync(`/Students/QuitStudentRoom/${idStudent}`);
            const dataResponse = response.data.data;

            return {
                data: dataResponse,
                result: response.data.result,
                message: response.data.messages[0]
            }

        } catch (error) {
            console.log(error)
            return {
                data: 0,
                result: false,
                message: "Error al activar o desactivar el estudiante."
            }
        }
    }

    // Private Methods
    private static GetFormDataUser(user: Student): FormData {
        const formData = new FormData();

        try {

            formData.append("id", user.id.toString() || "");
            formData.append("name", user.name || "");
            formData.append("lastname", user.lastname || "");
            formData.append("sexo", user.sexo?.toString() || "");
            formData.append("age", "0");
            formData.append("idInstitu", "0");
            formData.append("book", user.book || "");
            formData.append("folio", user.folio || "");
            formData.append("direction", user.direction || "");
            formData.append("telFather", user.telFather || "");
            formData.append("telMother", user.telMother || "");
            formData.append("idRoom", user.idRoom || "");
            formData.append("idTypeRegister", user.idTypeRegister.toString() || "0");
            formData.append("tel", user.tel || "");
            formData.append("relationPersonLiveWith", user.relationPersonLiveWith || "");
            formData.append("carriedPreprimary", user.carriedPreprimary.toString() || "");
            formData.append("neae", user.neae || "");
            formData.append("diseasesAllergic", user.diseasesAllergic || "");
            formData.append("medicinesUse", user.medicinesUse || "");
            formData.append("emergencyPerson", user.emergencyPerson || "");
            formData.append("emergencyTel", user.emergencyTel || "");
            formData.append("idFather", user.idFather || "");
            formData.append("idMother", user.idMother || "");
            formData.append("bornDate", user.bornDate.toString() || "");
            formData.append("idNacionality", user.idNacionality.toString() || "");
            formData.append("numberSiblings", user.numberSiblings || "");
            formData.append("agesSiblings", user.agesSiblings || "");
            formData.append("placeBetweenSiblings", user.placeBetweenSiblings || "");
            formData.append("doctorPediatrician", user.doctorPediatrician || "");
            formData.append("urlImg", user.urlImg || "");
            formData.append("idGrade", user.idGrade.toString() || "");
            formData.append("code", user.code || "");

            if (user.file) {
                formData.append("file", user.file);
            }
        } catch (error) {
            console.error("Error al crear el GetFormDataUserSudent:", error);
            return new FormData(); // Retorna un FormData vacío en caso de error
        }
        return formData;
    }


}

export default StudentService;



