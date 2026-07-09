import { create, StateCreator } from 'zustand'
import { ApiResponse } from '../../../interfaces/ApiResponse';
import { Student } from '../../../interfaces/Students/Student';
import StudentService from '../../../services/Student/Students.Service';
import { SearchStudent } from '../../../util/Student/SearchStudent';


interface StudentState {

    // Properties
    loading: boolean;
    getStudent: boolean;
    students: Student[];
    studentsParents: Student[];
    imutableStudents: Student[];
    changesStudents: Student[];
    currentDisplayStudents: Student[];
    currentPage: number;
    pageCount: number;
    searchTerm: string;

    // Actions
    GetDatosEstudianteByParents: (idParent: string) => Promise<ApiResponse<Student[]>>;
    getAllUsersAsync: (loadForce?: boolean, idParent?: string, isParentsView?: boolean) => Promise<ApiResponse<Student[]>>;
    onCreateOrUpdate: (student: Student) => Promise<ApiResponse<string>>;
    setPageCount: (count: number) => void;
    setSearchTerm: (searchTerm: string) => void;
    setcurrentDisplayStudents: (students: Student[], currentPage: number) => void;
    setChangesUsers: (students: Student[]) => void;
    setLoading: (statu: boolean) => void;
    onImportUsers: (file: File) => Promise<ApiResponse<string>>;
    onInactiveActive: (idStudent: number, estado: string) => Promise<ApiResponse<string>>;
}


const creatorStudent: StateCreator<StudentState> = (set, get) => ({

    searchTerm: '',
    loading: false,
    getStudent: false,
    currentPage: 0,
    pageCount: 0,
    imutableStudents: {} as Student[],
    students: {} as Student[],
    roles: {} as string[],
    changesStudents: {} as Student[],
    currentDisplayStudents: {} as Student[],
    studentsParents: {} as Student[],

    setLoading: (statu: boolean) => set(() => ({
        loading: statu
    })),
    setPageCount: (count: number) => set(() => ({
        pageCount: count
    })),
    setSearchTerm: (searchTerm: string) => set(() => ({
        searchTerm: searchTerm
    })),

    setcurrentDisplayStudents: (students: Student[], currentPage: number) => set((state) => ({
        ...state,
        currentPage: currentPage,
        currentDisplayStudents: students,
    })),
    setChangesUsers: (students: Student[]) => set((state) => ({
        ...state,
        changesStudents: students,
    })),

    GetDatosEstudianteByParents: async (idParent: string): Promise<ApiResponse<Student[]>> => {

        try {

            const dataReponse = await StudentService.GetDatosEstudianteByParents(idParent);

            const studentsParents = dataReponse.data;

            set((state) => {

                return {
                    ...state,
                    studentsParents,
                    loading: false,
                };

            });

            return dataReponse;

        } catch (error) {
            console.log(error)

            set(() => ({ loading: false }));
            return {} as ApiResponse<Student[]>
        }


    },
    getAllUsersAsync: async (loadForce?: boolean, idParent?: string, isParentsView?: boolean): Promise<ApiResponse<Student[]>> => {

        try {
            let dataUsers: Student[];
            let imutableStudents: Student[];
            let dataReponse: ApiResponse<Student[]>;
            const storeUsers = get().students;
            let searchTerm = get().searchTerm;
            let getStudent: boolean;

            if (isParentsView || loadForce || !Array.isArray(storeUsers) || storeUsers.length === 0) {
                set(() => ({ loading: true }));
                if (idParent) {
                    dataReponse = await StudentService.GetDatosEstudianteByParents(idParent);
                    getStudent = true;
                    searchTerm = '';
                } else {
                    dataReponse = await StudentService.GetAllStudentsAsync();
                    getStudent = false;
                }
                imutableStudents = dataReponse.data;
                dataUsers = dataReponse.data;
            } else {
                dataReponse = {} as ApiResponse<Student[]>;
                dataReponse.data = storeUsers;
                imutableStudents = storeUsers;
                dataUsers = storeUsers;
            }

            set((state) => {

                const currentPageIndex = state.currentPage;
                const currentPageStart = currentPageIndex * 10;
                const currentPageEnd = (currentPageIndex + 1) * 10;

                const currentDisplayStudents = dataUsers.slice(currentPageStart, currentPageEnd);

                return {
                    ...state,
                    searchTerm,
                    imutableStudents: imutableStudents,
                    students: dataUsers,
                    changesStudents: dataUsers,
                    currentDisplayStudents: currentDisplayStudents,
                    loading: false,
                    getStudent,
                    pageCount: Math.ceil(dataUsers.length / 10)
                };

            });

            return dataReponse;

        } catch (error) {
            console.log(error)

            set(() => ({ loading: false }));
            return {} as ApiResponse<Student[]>
        }

    },
    onImportUsers: async (file: File): Promise<ApiResponse<string>> => {
        try {

            // Realizar la llamada a la API
            const data = await StudentService.ImportUsers(file);
            return data;

        } catch (error) {
            console.error("Error during login:", error);
            set(() => ({ loading: false }));
            return {} as ApiResponse<string>;
        }
    },
    onCreateOrUpdate: async (student: Student): Promise<ApiResponse<string>> => {
        try {
            set(() => ({ loading: true }));

            const data = await StudentService.CreateOrUpdate(student);
            set((state) => {
                if (!Array.isArray(state.students) || state.students.length === 0) {
                    return { ...state, loading: false }; // No hay estudiantes, no hacer nada
                }
                const currentPageIndex = state.currentPage;
                const updatedStudents = [...state.students];

                const userIndex = updatedStudents.findIndex((u) => u.id === student.id);
                if (userIndex !== -1) {
                    // Si el usuario existe, actualizarlo
                    updatedStudents[userIndex] = student;
                } else {
                    // Si no existe, agregarlo
                    updatedStudents.push(student);
                }
                const currentPageStart = currentPageIndex * 10;
                const currentPageEnd = (currentPageIndex + 1) * 10;
                const currentDisplayUsers = updatedStudents.slice(currentPageStart, currentPageEnd);

                state.currentDisplayStudents = currentDisplayUsers;
                return {
                    ...state,
                    students: updatedStudents,
                    changesUsers: updatedStudents,
                    currentDisplayUsers,
                    pageCount: Math.ceil(updatedStudents.length / 10),
                    loading: false,
                };
            });

            return data;

        } catch (error) {
            console.error("Error during onCreateOrUpdate:", error);
            set(() => ({ loading: false }));
            return {} as ApiResponse<string>;
        }
    },
    onInactiveActive: async (idStudent: number, estado: string): Promise<ApiResponse<string>> => {
        try {
            set(() => ({ loading: true }));

            // Realizar la llamada a la API
            const data = await StudentService.ActiveInactive(idStudent);

            set((state) => {
                // Actualizar la lista de usuarios
                const updatedStudents = state.students.map((student) =>
                    student.id === idStudent
                        ? { ...student, estado: estado }
                        : student
                );
                const imutableStudentsUpdate = state.imutableStudents.map((student) =>
                    student.id === idStudent
                        ? { ...student, estado: estado }
                        : student
                );

                // Filtrar los usuarios si hay un término de búsqueda
                let filteredStudents = updatedStudents;
                if (state.searchTerm.trim() !== '') {
                    filteredStudents = SearchStudent({ students: updatedStudents, searchTerm: state.searchTerm });
                }

                // Calcular la cantidad de páginas y actualizar la lista de usuarios a mostrar
                const pageCount = Math.ceil(filteredStudents.length / 10);
                const currentDisplayStudents = filteredStudents.slice(state.currentPage * 10, (state.currentPage + 1) * 10);

                return {
                    ...state,
                    students: updatedStudents,
                    changesStudents: updatedStudents,
                    currentDisplayStudents,
                    imutableStudents: imutableStudentsUpdate,
                    pageCount,
                    loading: false,
                };
            });

            return data;

        } catch (error) {
            console.error("Error during login:", error);
            set(() => ({ loading: false }));
            return {} as ApiResponse<string>;
        }
    },

})



export const useStudentStore = create<StudentState>()(
    creatorStudent
);
