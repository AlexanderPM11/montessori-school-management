import { create, StateCreator } from "zustand";
import { ApiResponse } from "../../../interfaces/ApiResponse";
import { RoomSubject } from "../../../interfaces/Room/RoomSubject";
import RoomSubjectService from "../../../services/Room/RoomSubjectService";

interface RoomSubjectState {
    // Properties
    loading: boolean;
    roomSubjet: RoomSubject[];
    changesRoomSubjects: RoomSubject[];
    currentDisplayRoomSubjects: RoomSubject[];
    currentPage: number;
    pageCount: number;
    searchTerm: string;

    // Actions
    setPageCount: (count: number) => void;
    setSearchTerm: (searchTerm: string) => void;
    setcurrentDisplayRoomSubject: (
        Rooms: RoomSubject[],
        currentPage: number
    ) => void;
    setChangesRoomSubject: (Rooms: RoomSubject[]) => void;
    setLoading: (statu: boolean) => void;

    onDelete: (idRoom: number) => Promise<ApiResponse<number>>;
    GetAllRoomSubjet: (idRoom: number) => Promise<ApiResponse<RoomSubject[]>>;
    onCreateOrUpdate: (roomSubject: RoomSubject) => Promise<ApiResponse<number>>;
}

const creatorRoomSubject: StateCreator<RoomSubjectState> = (set, get) => ({
    loading: false,
    roomSubjet: {} as RoomSubject[],
    changesRoomSubjects: {} as RoomSubject[],
    currentDisplayRoomSubjects: {} as RoomSubject[],
    currentPage: 0,
    pageCount: 0,
    searchTerm: "",

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

    setcurrentDisplayRoomSubject: (Rooms: RoomSubject[], currentPage: number) =>
        set((state) => ({
            ...state,
            currentPage: currentPage,
            currentDisplayRoomSubjects: Rooms,
        })),
    setChangesRoomSubject: (Rooms: RoomSubject[]) =>
        set((state) => ({
            ...state,
            changesRoomSubjects: Rooms,
        })),
    GetAllRoomSubjet: async (
        idRoom: number
    ): Promise<ApiResponse<RoomSubject[]>> => {
        try {
            let dataRoomSubjects: RoomSubject[];
            let dataReponse: ApiResponse<RoomSubject[]>;
            const storeRoomSubjects = get().roomSubjet;

            if (!Array.isArray(storeRoomSubjects) || storeRoomSubjects.length === 0) {
                set(() => ({ loading: true }));
                dataReponse = await RoomSubjectService.GetAllRoomTeacher(idRoom);
                dataRoomSubjects = dataReponse.data;
            } else {
                dataReponse = {} as ApiResponse<RoomSubject[]>;
                dataReponse.data = storeRoomSubjects;
                dataRoomSubjects = storeRoomSubjects;
            }

            set((state) => {
                const currentPageIndex = state.currentPage;
                const currentPageStart = currentPageIndex * 10;
                const currentPageEnd = (currentPageIndex + 1) * 10;

                const currentDisplayRoomSubjects = dataRoomSubjects.slice(
                    currentPageStart,
                    currentPageEnd
                );

                return {
                    ...state,
                    roomSubjet: dataRoomSubjects,
                    changesRoomSubjects: dataRoomSubjects,
                    currentDisplayRoomSubjects: currentDisplayRoomSubjects,
                    loading: false,
                    pageCount: Math.ceil(dataRoomSubjects.length / 10),
                };
            });

            return dataReponse;
        } catch (error) {
            console.error("Error during login:", error);
            set(() => ({ loading: false }));
            return {} as ApiResponse<RoomSubject[]>;
        }
    },
    onCreateOrUpdate: async (
        roomSubject: RoomSubject
    ): Promise<ApiResponse<number>> => {
        try {
            set(() => ({ loading: true }));

            const data = await RoomSubjectService.CreateOrUpdate(roomSubject);
            set((state) => {
                if (!Array.isArray(state.roomSubjet) || state.roomSubjet.length === 0) {
                    return { ...state, loading: false };
                }

                const currentPageIndex = state.currentPage;
                const updatedRooms = [...state.roomSubjet];

                const userIndex = updatedRooms.findIndex(
                    (u) => u.id === roomSubject.id
                );
                if (userIndex !== -1) {
                    // Si el usuario existe, actualizarlo
                    updatedRooms[userIndex] = roomSubject;
                } else {
                    // Si no existe, agregarlo
                    roomSubject.id = data.data;
                    updatedRooms.push(roomSubject);
                }

                const currentPageStart = currentPageIndex * 10;
                const currentPageEnd = (currentPageIndex + 1) * 10;
                const currentDisplayRooms = updatedRooms.slice(
                    currentPageStart,
                    currentPageEnd
                );

                return {
                    ...state,
                    roomSubjet: updatedRooms,
                    changesRoomSubjects: updatedRooms,
                    currentDisplayRoomSubjects: currentDisplayRooms,
                    pageCount: Math.ceil(updatedRooms.length / 10),
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

            const data = await RoomSubjectService.Delete(idRoom);

            // Si la eliminación en el servidor fue exitosa, eliminar localmente
            if (data.result) {
                set((state) => {
                    const updatedroomSubjet = state.roomSubjet.filter(
                        (adjunto) => adjunto.id !== idRoom
                    );
                    return {
                        ...state,
                        roomSubjet: updatedroomSubjet,
                        changesRoomSubjects: updatedroomSubjet,
                        currentDisplayRoomSubjects: updatedroomSubjet.slice(
                            state.currentPage * 10,
                            (state.currentPage + 1) * 10
                        ),
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

export const roomSubjectStore = create<RoomSubjectState>()(creatorRoomSubject);
