import { create, StateCreator } from "zustand";
import { ApiResponse } from "../../../interfaces/ApiResponse";
import { Room } from "../../../interfaces/Room/Room";
import RoomService from "../../../services/Room/RoomService";
import { itemsPerPage } from "../../../components/rooms/generals";

interface RoomTeacherstate {
    // Properties
    loading: boolean;
    getRoom: boolean;
    rooms: Room[];
    imutableRooms: Room[];
    changesRooms: Room[];
    currentDisplayRooms: Room[];
    currentPage: number;
    pageCount: number;
    searchTerm: string;

    // Actions
    GetAllRoomTeacher: () => Promise<ApiResponse<Room[]>>;
    onCreateOrUpdate: (room: Room) => Promise<ApiResponse<number>>;
    onDelete: (idRoom: number) => Promise<ApiResponse<number>>;

    setPageCount: (count: number) => void;
    setSearchTerm: (searchTerm: string) => void;
    setcurrentDisplayRooms: (Rooms: Room[], currentPage: number) => void;
    setChangesRooms: (Rooms: Room[]) => void;
    setLoading: (statu: boolean) => void;
}

const creatorRoomTeacher: StateCreator<RoomTeacherstate> = (set, get) => ({
    searchTerm: "",
    loading: true,
    getRoom: false,
    currentPage: 0,
    pageCount: 0,
    imutableRooms: {} as Room[],
    rooms: {} as Room[],
    roles: {} as string[],
    changesRooms: {} as Room[],
    currentDisplayRooms: {} as Room[],
    RoomsParents: {} as Room[],

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

    setcurrentDisplayRooms: (Rooms: Room[], currentPage: number) =>
        set((state) => ({
            ...state,
            currentPage: currentPage,
            currentDisplayRooms: Rooms,
        })),
    setChangesRooms: (Rooms: Room[]) =>
        set((state) => ({
            ...state,
            changesRooms: Rooms,
        })),
    onCreateOrUpdate: async (room: Room): Promise<ApiResponse<number>> => {
        try {
            set(() => ({ loading: true }));

            const data = await RoomService.CreateOrUpdate(room);
            room.id = data.data;
            room.level = room.levelBack.toString();
            room.idTypeRegisters = room.idTypeRegistersBack.toString();
            room.idTypeRegistersList = room.idTypeRegistersBack
                .split(",")
                .map((x: string) => x.trim());

            set((state) => {
                if (!Array.isArray(state.rooms) || state.rooms.length === 0) {
                    return { ...state, loading: false };
                }
                const currentPageIndex = state.currentPage;
                const updatedRooms = [...state.rooms];

                const userIndex = updatedRooms.findIndex((u) => u.id === room.id);
                if (userIndex !== -1) {
                    // Si el usuario existe, actualizarlo
                    updatedRooms[userIndex] = room;
                } else {
                    // Si no existe, agregarlo

                    room.created = new Date().toISOString();
                    updatedRooms.push(room);
                }
                const currentPageStart = currentPageIndex * itemsPerPage;
                const currentPageEnd = (currentPageIndex + 1) * itemsPerPage;
                const currentDisplayRooms = updatedRooms.slice(
                    currentPageStart,
                    currentPageEnd
                );

                return {
                    ...state,
                    rooms: updatedRooms,
                    changesRooms: updatedRooms,
                    currentDisplayRooms,
                    pageCount: Math.ceil(updatedRooms.length / itemsPerPage),
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
    GetAllRoomTeacher: async (): Promise<ApiResponse<Room[]>> => {
        try {
            let dataRooms: Room[];
            let imutableRooms: Room[];
            let dataReponse: ApiResponse<Room[]>;
            const storeRooms = get().rooms;
            if (!Array.isArray(storeRooms) || storeRooms.length === 0) {
                set(() => ({ loading: true }));
                dataReponse = await RoomService.GetAllRooms();
                imutableRooms = dataReponse.data;
                dataRooms = dataReponse.data;
            } else {
                dataReponse = {} as ApiResponse<Room[]>;
                dataReponse.data = storeRooms;
                imutableRooms = storeRooms;
                dataRooms = storeRooms;
            }

            set((state) => {
                const currentPageIndex = state.currentPage;
                const currentPageStart = currentPageIndex * itemsPerPage;
                const currentPageEnd = (currentPageIndex + 1) * itemsPerPage;

                const currentDisplayRooms = dataRooms.slice(
                    currentPageStart,
                    currentPageEnd
                );

                return {
                    ...state,
                    imutableRooms: imutableRooms,
                    rooms: dataRooms,
                    changesRooms: dataRooms,
                    currentDisplayRooms: currentDisplayRooms,
                    loading: false,
                    pageCount: Math.ceil(dataRooms.length / itemsPerPage),
                };
            });

            return dataReponse;
        } catch (error) {
            console.log(error);

            set(() => ({ loading: false }));
            return {} as ApiResponse<Room[]>;
        }
    },
    onDelete: async (idRoom: number): Promise<ApiResponse<number>> => {
        try {
            set(() => ({ loading: true }));

            const data = await RoomService.Delete(idRoom);

            // Si la eliminación en el servidor fue exitosa, eliminar localmente
            if (data.result) {
                set((state) => {
                    const updatedRooms = state.rooms.filter(
                        (room) => room.id !== idRoom
                    );
                    return {
                        ...state,
                        changesRooms: updatedRooms,
                        currentDisplayRooms: updatedRooms,
                        rooms: updatedRooms,
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

export const useRoomTeacherStore = create<RoomTeacherstate>()(creatorRoomTeacher);
