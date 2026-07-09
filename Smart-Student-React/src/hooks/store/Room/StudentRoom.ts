import { create, StateCreator } from "zustand";
import { ApiResponse } from "../../../interfaces/ApiResponse";
import { Student } from "../../../interfaces/Students/Student";
import StudentService from "../../../services/Student/Students.Service";
import { QuitAddStudentRoom } from "../../../interfaces/Room/QuitAddStudentRoom";
import { SearchStudent } from "../../../util/Student/SearchStudent";

interface StudentState {
    // Properties
    loading: boolean;
    // To  add room
    StudentsToAddRoom: Student[];
    StudentsToAddRoomInmutable: Student[];
    changesStudentsToAddRoom: Student[];
    currentDisplayUsersToAddRoom: Student[];
    currentPageToAddRoom: number;
    pageCountToAddRoom: number;
    searchTermToAddRoom: string;

    // Rooms Desplay
    Students: Student[];
    StudentsInmutable: Student[];
    changesStudents: Student[];
    currentDisplayStudents: Student[];
    currentPage: number;
    pageCount: number;
    searchTerm: string;

    // Actions
    setPageCountToAddRoom: (count: number) => void;
    setSearchTermToAddRoom: (searchTermToAddRoom: string) => void;
    setCurrentDisplayStudentsToAddRoom: (
        Students: Student[],
        currentPageToAddRoom: number
    ) => void;
    setChangesStudentsToAddRoom: (Students: Student[]) => void;
    setLoading: (statu: boolean) => void;

    setPageCountRoom: (count: number) => void;
    setSearchTermRoom: (searchTermToAddRoom: string) => void;
    setCurrentDisplayStudents: (Students: Student[], currentPage: number) => void;
    setChangesStudents: (Students: Student[]) => void;
    setLoadingRoom: (statu: boolean) => void;
    setCurrentPageToAddRoom: (page: number) => void;
    setCurrentPage: (page: number) => void;
    getFilteredStudentsToAddRoom: () => void;
    getFilteredStudentsRoom: () => void;

    GetStudentsRoom: (idRoom: number) => Promise<ApiResponse<Student[]>>;
    GetStudentsToAddRoom: (idRoom: number) => Promise<ApiResponse<Student[]>>;
    AddStudentRoom: (data: QuitAddStudentRoom) => Promise<ApiResponse<number>>;
    QuitStudentRoom: (idStudent: number) => Promise<ApiResponse<number>>;
}

const creatorUser: StateCreator<StudentState> = (set, get) => ({
    searchTermToAddRoom: "",
    loading: false,
    currentPageToAddRoom: 0,
    pageCountToAddRoom: 0,
    StudentsToAddRoom: {} as Student[],
    StudentsToAddRoomInmutable: {} as Student[],
    changesStudentsToAddRoom: {} as Student[],
    currentDisplayUsersToAddRoom: {} as Student[],

    searchTerm: "",
    currentPage: 0,
    pageCount: 0,
    StudentsInmutable: {} as Student[],
    Students: {} as Student[],
    changesStudents: {} as Student[],
    currentDisplayStudents: {} as Student[],

    setLoading: (statu: boolean) =>
        set(() => ({
            loading: statu,
        })),
    setCurrentPageToAddRoom: (page: number) =>
        set(() => ({
            currentPageToAddRoom: page,
        })),
    setPageCountToAddRoom: (count: number) =>
        set(() => ({
            pageCountToAddRoom: count,
        })),
    setSearchTermToAddRoom: (searchTermToAddRoom: string) =>
        set(() => ({
            searchTermToAddRoom: searchTermToAddRoom,
        })),

    setCurrentDisplayStudentsToAddRoom: (
        users: Student[],
        currentPageToAddRoom: number
    ) =>
        set((state) => ({
            ...state,
            currentPageToAddRoom: currentPageToAddRoom,
            currentDisplayUsersToAddRoom: users,
        })),
    setChangesStudentsToAddRoom: (users: Student[]) =>
        set((state) => ({
            ...state,
            changesStudentsToAddRoom: users,
        })),

    // Rooms Desplay
    setCurrentPage: (page: number) =>
        set(() => ({
            currentPage: page,
        })),
    setLoadingRoom: (statu: boolean) =>
        set(() => ({
            loading: statu,
        })),
    setPageCountRoom: (count: number) =>
        set(() => ({
            pageCount: count,
        })),
    setSearchTermRoom: (searchTerm: string) =>
        set(() => ({
            searchTerm: searchTerm,
        })),

    setCurrentDisplayStudents: (users: Student[], currentPage: number) =>
        set((state) => ({
            ...state,
            currentPage: currentPage,
            currentDisplayStudents: users,
        })),
    setChangesStudents: (users: Student[]) =>
        set((state) => ({
            ...state,
            changesStudents: users,
        })),
    GetStudentsRoom: async (idRoom: number): Promise<ApiResponse<Student[]>> => {
        try {
            let dataUsers: Student[];
            let dataReponse: ApiResponse<Student[]>;
            const storeUsers = get().Students;

            if (!Array.isArray(storeUsers) || storeUsers.length === 0) {
                set(() => ({ loading: true }));
                dataReponse = await StudentService.GetStudentsRoom(idRoom);
                dataUsers = dataReponse.data;
            } else {
                dataReponse = {} as ApiResponse<Student[]>;
                dataReponse.data = storeUsers;
                dataUsers = storeUsers;
            }
            set((state) => {
                const currentPage = state.currentPage;
                const currentPageStart = currentPage * 10;
                const currentPageEnd = (currentPage + 1) * 10;

                const currentDisplayStudents = dataUsers.slice(
                    currentPageStart,
                    currentPageEnd
                );

                return {
                    ...state,
                    StudentsInmutable: dataUsers,
                    Students: dataUsers,
                    changesStudents: dataUsers,
                    currentDisplayStudents,
                    loading: false,
                    pageCount: Math.ceil(dataUsers.length / 10),
                };
            });

            return dataReponse;
        } catch (error) {
            console.error("Error during login:", error);
            set(() => ({ loading: false }));
            return {} as ApiResponse<Student[]>;
        }
    },
    GetStudentsToAddRoom: async (
        idRoom: number
    ): Promise<ApiResponse<Student[]>> => {
        try {
            let dataUsers: Student[];
            let dataReponse: ApiResponse<Student[]>;
            const storeUsers = get().StudentsToAddRoom;

            if (!Array.isArray(storeUsers) || storeUsers.length === 0) {
                set(() => ({ loading: true }));
                dataReponse = await StudentService.GetStudentsToAddRoom(idRoom);
                dataUsers = dataReponse.data;
            } else {
                dataReponse = {} as ApiResponse<Student[]>;
                dataReponse.data = storeUsers;
                dataUsers = storeUsers;
            }
            set((state) => {
                const currentToAddRoom = state.currentPageToAddRoom;
                const currentToAddRoomStart = currentToAddRoom * 10;
                const currentToAddRoomEnd = (currentToAddRoom + 1) * 10;

                const currentDisplayUsersToAddRoom = dataUsers.slice(
                    currentToAddRoomStart,
                    currentToAddRoomEnd
                );

                return {
                    ...state,
                    StudentsToAddRoom: dataUsers,
                    StudentsToAddRoomInmutable: dataUsers,
                    changesStudentsToAddRoom: dataUsers,
                    currentDisplayUsersToAddRoom,
                    loading: false,
                    pageCount: Math.ceil(dataUsers.length / 10),
                };
            });

            return dataReponse;
        } catch (error) {
            console.error("Error during login:", error);
            set(() => ({ loading: false }));
            return {} as ApiResponse<Student[]>;
        }
    },
    AddStudentRoom: async (
        data: QuitAddStudentRoom
    ): Promise<ApiResponse<number>> => {
        try {
            const dataResposne = await StudentService.AddStudentRoom(data);
            let newCurrentPageToAddRoom: number = 0;

            if (dataResposne.result) {
                set((state) => {
                    const currentToAddRoom = Array.isArray(state.changesStudentsToAddRoom)
                        ? state.changesStudentsToAddRoom
                        : [];

                    const currentTeachers = Array.isArray(state.Students)
                        ? state.Students
                        : [];

                    const updatedToAdd = currentToAddRoom.filter(
                        (user) => !data.idStudents.includes(user.id)
                    );
                    const updatedToAddInmutable = state.StudentsToAddRoomInmutable.filter(
                        (user) => !data.idStudents.includes(user.id)
                    );

                    const addedTeachers = currentToAddRoom.filter((user) =>
                        data.idStudents.includes(user.id)
                    );

                    const updatedInRoom =
                        currentTeachers.length > 0
                            ? [...currentTeachers, ...addedTeachers]
                            : currentTeachers;
                    const updatedInRoomSorted = updatedInRoom
                        .slice()
                        .sort((a, b) => a.lastname.localeCompare(b.lastname))
                        .map((item, index) => ({
                            ...item,
                            numberList: index + 1,
                        }));

                    if (currentToAddRoom.length > 0) {
                        const totalUpdatedToAddPages = Math.ceil(updatedToAdd.length / 10);
                        newCurrentPageToAddRoom = Math.min(
                            state.currentPageToAddRoom,
                            totalUpdatedToAddPages - 1
                        );
                    }
                    return {
                        ...state,
                        // Actualizamos los profesores para agregar
                        StudentsToAddRoom: updatedToAdd,
                        StudentsToAddRoomInmutable: updatedToAddInmutable,
                        changesStudentsToAddRoom: updatedToAdd,
                        currentDisplayUsersToAddRoom: updatedToAdd.slice(
                            newCurrentPageToAddRoom * 10,
                            (newCurrentPageToAddRoom + 1) * 10
                        ),
                        pageCountToAddRoom: Math.ceil(updatedToAdd.length / 10),
                        currentPageToAddRoom: newCurrentPageToAddRoom,

                        // Actualizamos los profesores que ya están en la sala
                        Students: updatedInRoomSorted,
                        changesStudents: updatedInRoomSorted,
                        currentDisplayStudents: updatedInRoomSorted.slice(
                            state.currentPage * 10,
                            (state.currentPage + 1) * 10
                        ),
                        searchTermToAddRoom: "",
                        searchTerm: "",
                        loading: false,
                    };
                });
            }

            return dataResposne;
        } catch (error) {
            console.error("Error:", error);
            set(() => ({ loading: false }));
            return {} as ApiResponse<number>;
        }
    },
    QuitStudentRoom: async (idStudent: number): Promise<ApiResponse<number>> => {
        try {
            const dataResposne = await StudentService.QuitStudentRoom(idStudent);
            let newCurrentPage: number = 0;

            if (dataResposne.result) {
                set((state) => {
                    const currentChangesUsers = Array.isArray(state.changesStudents)
                        ? state.changesStudents
                        : [];

                    const currentToAddRoom = Array.isArray(state.StudentsToAddRoom)
                        ? state.StudentsToAddRoom
                        : [];

                    if (
                        currentChangesUsers.length === 0 &&
                        currentToAddRoom.length === 0
                    ) {
                        return {
                            ...state,
                            loading: false,
                        };
                    }

                    const updatedInRoom = currentChangesUsers.filter(
                        (user) => user.id != idStudent
                    );
                    const updatedInmutable = state.StudentsInmutable.filter(
                        (user) => user.id != idStudent
                    );

                    const removedTeachers = currentChangesUsers.filter(
                        (user) => user.id === idStudent
                    );

                    const updatedToAdd = [...currentToAddRoom, ...removedTeachers];

                    if (updatedInRoom.length > 0) {
                        const totalUpdatedToAddPages = Math.ceil(updatedInRoom.length / 10);
                        newCurrentPage = Math.min(
                            state.currentPage,
                            totalUpdatedToAddPages - 1
                        );
                    }
                    return {
                        ...state,
                        StudentsInmutable: updatedInmutable,
                        Students: updatedInRoom,
                        changesStudents: updatedInRoom,
                        currentDisplayStudents: updatedInRoom.slice(
                            newCurrentPage * 10,
                            (newCurrentPage + 1) * 10
                        ),
                        pageCount: Math.ceil(updatedInRoom.length / 10),
                        currentPage: newCurrentPage,
                        ...(currentToAddRoom.length > 0 && {
                            StudentsToAddRoom: updatedToAdd,
                            changesStudentsToAddRoom: updatedToAdd,
                            currentDisplayUsersToAddRoom: updatedToAdd.slice(
                                state.currentPageToAddRoom * 10,
                                (state.currentPageToAddRoom + 1) * 10
                            ),
                            pageCountToAddRoom: Math.ceil(updatedToAdd.length / 10),
                        }),
                        searchTermToAddRoom: "",
                        searchTerm: "",
                        loading: false,
                    };
                });
            }

            return dataResposne;
        } catch (error) {
            console.error("Error during quit teacher room:", error);
            set(() => ({ loading: false }));
            return {} as ApiResponse<number>;
        }
    },
    getFilteredStudentsToAddRoom: async () => {
        try {
            set((state) => {
                const searchTerm = state.searchTermToAddRoom.trim();
                const studentsRaw = state.StudentsToAddRoomInmutable;

                const filteredStudents = SearchStudent({
                    students: studentsRaw,
                    searchTerm,
                });

                const pageCount = Math.ceil(filteredStudents.length / 10);
                const currentPageIndex = 0; // Reiniciamos a la primera página
                const currentPageStart = currentPageIndex * 10;
                const currentPageEnd = currentPageStart + 10;

                const currentDisplayStudents = filteredStudents.slice(
                    currentPageStart,
                    currentPageEnd
                );

                return {
                    ...state,
                    StudentsToAddRoom: studentsRaw,
                    changesStudentsToAddRoom: filteredStudents,
                    currentDisplayUsersToAddRoom: currentDisplayStudents,
                    loading: false,
                    pageCount,
                    currentPageToAddRoom: currentPageIndex,
                };
            });
        } catch (error) {
            console.error("Error during filtering students to add room:", error);
            set(() => ({ loading: false }));
        }
    },
    getFilteredStudentsRoom: async () => {
        try {
            set((state) => {
                const searchTerm = state.searchTerm.trim();
                const studentsRaw = state.StudentsInmutable;

                const filteredStudents = SearchStudent({
                    students: studentsRaw,
                    searchTerm,
                });

                const pageCount = Math.ceil(filteredStudents.length / 10);
                const currentPageIndex = 0; // Reiniciamos a la primera página
                const currentPageStart = currentPageIndex * 10;
                const currentPageEnd = currentPageStart + 10;

                const currentDisplayStudents = filteredStudents.slice(
                    currentPageStart,
                    currentPageEnd
                );

                return {
                    ...state,
                    Students: studentsRaw,
                    changesStudents: filteredStudents,
                    currentDisplayStudents: currentDisplayStudents,
                    loading: false,
                    pageCount,
                    currentPage: currentPageIndex,
                };
            });
        } catch (error) {
            console.error("Error during filtering students to add room:", error);
            set(() => ({ loading: false }));
        }
    },

    // Student to add room
});

export const useStudentRoomStore = create<StudentState>()(creatorUser);
