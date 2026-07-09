import { create, StateCreator } from "zustand";
import { ApiResponse } from "../../../interfaces/ApiResponse";
import { Subject } from "../../../interfaces/Subject/Subject";
import SubjectService from "../../../services/Subject/SubjectService";

interface SubjectState {
    // Properties
    loading: boolean;
    subjects: Subject[];
    changesSubjects: Subject[];
    currentDisplaySubjects: Subject[];
    currentPage: number;
    pageCount: number;
    searchTerm: string;

    // Actions
    getSubject: () => Promise<ApiResponse<Subject[]>>;
    onCreateOrUpdate: (Subject: Subject) => Promise<ApiResponse<number>>;
    onDelete: (idRoom: number) => Promise<ApiResponse<number>>;
    setPageCount: (count: number) => void;
    setSearchTerm: (searchTerm: string) => void;
    setcurrentDisplaySubjects: (Subjects: Subject[], currentPage: number) => void;
    setChangesSubjects: (Subjects: Subject[]) => void;
    setLoading: (statu: boolean) => void;
}

const creatorSubject: StateCreator<SubjectState> = (set, get) => ({
    searchTerm: "",
    loading: false,
    currentPage: 0,
    pageCount: 0,
    subjects: {} as Subject[],
    changesSubjects: {} as Subject[],
    currentDisplaySubjects: {} as Subject[],

    setLoading: (statu: boolean) =>
        set(() => ({
            loading: statu,
        })),
    setPageCount: (count: number) =>
        set(() => ({
            pageCount: count,
        })),
    setSearchTerm: (searchTerm: string) =>
        set(() => ({
            searchTerm: searchTerm,
        })),

    setcurrentDisplaySubjects: (Subjects: Subject[], currentPage: number) =>
        set((state) => ({
            ...state,
            currentPage: currentPage,
            currentDisplaySubjects: Subjects,
        })),
    setChangesSubjects: (Subjects: Subject[]) =>
        set((state) => ({
            ...state,
            changesSubjects: Subjects,
        })),
    getSubject: async (): Promise<ApiResponse<Subject[]>> => {
        try {
            let dataUsers: Subject[];
            let imutableSubjects: Subject[];
            let dataReponse: ApiResponse<Subject[]>;
            const storeUsers = get().subjects;

            if (!Array.isArray(storeUsers) || storeUsers.length === 0) {
                set(() => ({ loading: true }));

                dataReponse = await SubjectService.GetSubject();
                imutableSubjects = dataReponse.data;
                dataUsers = dataReponse.data;
            } else {
                dataReponse = {} as ApiResponse<Subject[]>;
                dataReponse.data = storeUsers;
                imutableSubjects = storeUsers;
                dataUsers = storeUsers;
            }

            set((state) => {
                const currentPageIndex = state.currentPage;
                const currentPageStart = currentPageIndex * 10;
                const currentPageEnd = (currentPageIndex + 1) * 10;

                const currentDisplaySubjects = dataUsers.slice(
                    currentPageStart,
                    currentPageEnd
                );

                return {
                    ...state,
                    imutableSubjects: imutableSubjects,
                    subjects: dataUsers,
                    changesSubjects: dataUsers,
                    currentDisplaySubjects: currentDisplaySubjects,
                    loading: false,
                    pageCount: Math.ceil(dataUsers.length / 10),
                };
            });

            return dataReponse;
        } catch (error) {
            console.log(error);

            set(() => ({ loading: false }));
            return {} as ApiResponse<Subject[]>;
        }
    },

    onCreateOrUpdate: async (subject: Subject): Promise<ApiResponse<number>> => {
        try {
            set(() => ({ loading: true }));

            const data = await SubjectService.CreateOrUpdate(subject);
            set((state) => {
                if (!Array.isArray(state.subjects) || state.subjects.length === 0) {
                    return { ...state, loading: false };
                }
                const currentPageIndex = state.currentPage;
                const updatedSubjects = [...state.subjects];

                const userIndex = updatedSubjects.findIndex((u) => u.id === subject.id);
                subject.imageUrl = subject.imageUrl || "";
                if (userIndex !== -1) {
                    updatedSubjects[userIndex] = subject;
                } else {
                    subject.id = data.data;
                    updatedSubjects.push(subject);
                }
                const currentPageStart = currentPageIndex * 10;
                const currentPageEnd = (currentPageIndex + 1) * 10;
                const currentDisplaySubjects = updatedSubjects.slice(
                    currentPageStart,
                    currentPageEnd
                );

                state.currentDisplaySubjects = currentDisplaySubjects;
                return {
                    ...state,
                    subjects: updatedSubjects,
                    changesSubjects: updatedSubjects,
                    currentDisplaySubjects: currentDisplaySubjects,
                    pageCount: Math.ceil(updatedSubjects.length / 10),
                    loading: false,
                };
            });

            return data;
        } catch (error) {
            console.error("Error during onCreateOrUpdate:", error);
            set(() => ({ loading: false }));
            return {} as ApiResponse<number>;
        }
    },
    onDelete: async (idRoom: number): Promise<ApiResponse<number>> => {
        try {
            set(() => ({ loading: true }));

            const data = await SubjectService.Delete(idRoom);

            // Si la eliminación en el servidor fue exitosa, eliminar localmente
            if (data.result) {
                set((state) => {
                    const updatedRooms = state.subjects.filter(
                        (room) => room.id !== idRoom
                    );
                    return {
                        ...state,
                        changesSubjects: updatedRooms,
                        currentDisplaySubjects: updatedRooms,
                        subjects: updatedRooms,
                        loading: false,
                    };
                });
            } else {
                set(() => ({ loading: false }));
            }

            return data;
        } catch (error) {
            console.error("Error during onDelete:", error);
            set(() => ({ loading: false }));
            return {} as ApiResponse<number>;
        }
    },
});

export const useSubjectStore = create<SubjectState>()(creatorSubject);
