import { create, StateCreator } from "zustand";
import { ApiResponse } from "../../../interfaces/ApiResponse";
import { User } from "../../../interfaces/Auth/User";
import UserService from "../../../services/Users/Users.Service";
import { QuitAddTeacherRoom } from "../../../interfaces/Room/QuitAddTeacherRoom";

interface UserState {
    // Properties
    loading: boolean;
    // To  add room
    teacherToAddRoom: User[];
    changesUsersToAddRoom: User[];
    currentDisplayUsersToAddRoom: User[];
    currentPageToAddRoom: number;
    pageCountToAddRoom: number;
    searchTermToAddRoom: string;

    // Rooms Desplay
    teacher: User[];
    changesUsers: User[];
    currentDisplayUsers: User[];
    currentPage: number;
    pageCount: number;
    searchTerm: string;
    lastFetchedRoomId: number | null;

    // Actions
    setPageCountToAddRoom: (count: number) => void;
    setSearchTermToAddRoom: (searchTermToAddRoom: string) => void;
    setCurrentDisplayUsersToAddRoom: (
        users: User[],
        currentPageToAddRoom: number
    ) => void;
    setChangesUsersToAddRoom: (users: User[]) => void;
    setLoading: (statu: boolean) => void;
    setLastFetchedRoomId: (id: number) => void;

    setPageCountRoom: (count: number) => void;
    setSearchTermRoom: (searchTermToAddRoom: string) => void;
    setCurrentDisplayUsers: (users: User[], currentPageToAddRoom: number) => void;
    setChangesUsers: (users: User[]) => void;
    setLoadingRoom: (statu: boolean) => void;

    GetTeacherToAddRoom: (idRoom: number) => Promise<ApiResponse<User[]>>;
    GetTeacherRoom: (idRoom: number) => Promise<ApiResponse<User[]>>;
    AddTeacherRoom: (data: QuitAddTeacherRoom) => Promise<ApiResponse<string>>;
    QuitTeacherRoom: (data: QuitAddTeacherRoom) => Promise<ApiResponse<string>>;
}

const creatorUser: StateCreator<UserState> = (set, get) => ({
    searchTermToAddRoom: "",
    loading: false,
    currentPageToAddRoom: 0,
    pageCountToAddRoom: 0,
    teacherToAddRoom: {} as User[],
    teachersToAddRoom: {} as User[],
    changesUsersToAddRoom: {} as User[],
    currentDisplayUsersToAddRoom: {} as User[],

    currentDisplayUsers: {} as User[],
    searchTerm: "",
    currentPage: 0,
    pageCount: 0,
    teacher: {} as User[],
    teachers: {} as User[],
    changesUsers: {} as User[],
    lastFetchedRoomId: null,
    setLastFetchedRoomId: (id: number) => set(() => ({ lastFetchedRoomId: id })),

    setLoading: (statu: boolean) =>
        set(() => ({
            loading: statu,
        })),
    setPageCountToAddRoom: (count: number) =>
        set(() => ({
            pageCountToAddRoom: count,
        })),
    setSearchTermToAddRoom: (searchTermToAddRoom: string) =>
        set(() => ({
            searchTermToAddRoom: searchTermToAddRoom,
        })),

    setCurrentDisplayUsersToAddRoom: (
        users: User[],
        currentPageToAddRoom: number
    ) =>
        set((state) => ({
            ...state,
            currentPageToAddRoom: currentPageToAddRoom,
            currentDisplayUsersToAddRoom: users,
        })),
    setChangesUsersToAddRoom: (users: User[]) =>
        set((state) => ({
            ...state,
            changesUsersToAddRoom: users,
        })),
    // Rooms Desplay
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

    setCurrentDisplayUsers: (users: User[], currentPage: number) =>
        set((state) => ({
            ...state,
            currentPage: currentPage,
            currentDisplayUsers: users,
        })),
    setChangesUsers: (users: User[]) =>
        set((state) => ({
            ...state,
            changesUsers: users,
        })),
    GetTeacherToAddRoom: async (idRoom: number): Promise<ApiResponse<User[]>> => {
        try {
            let dataUsers: User[];
            let dataReponse: ApiResponse<User[]>;
            const storeUsers = get().teacherToAddRoom;

            if (!Array.isArray(storeUsers) || storeUsers.length === 0) {
                set(() => ({ loading: true }));
                dataReponse = await UserService.GetTeacherToAddRoom(idRoom);
                dataUsers = dataReponse.data;
            } else {
                dataReponse = {} as ApiResponse<User[]>;
                dataReponse.data = storeUsers;
                dataUsers = storeUsers;
            }

            set((state) => {
                const currentPageToAddRoomIndex = state.currentPageToAddRoom;
                const currentPageToAddRoomStart = currentPageToAddRoomIndex * 10;
                const currentPageToAddRoomEnd = (currentPageToAddRoomIndex + 1) * 10;

                const currentDisplayUsersToAddRoom = dataUsers.slice(
                    currentPageToAddRoomStart,
                    currentPageToAddRoomEnd
                );

                return {
                    ...state,
                    teacherToAddRoom: dataUsers,
                    changesUsersToAddRoom: dataUsers,
                    currentDisplayUsersToAddRoom: currentDisplayUsersToAddRoom,
                    loading: false,
                    pageCountToAddRoom: Math.ceil(dataUsers.length / 10),
                };
            });

            return dataReponse;
        } catch (error) {
            console.error("Error during login:", error);
            set(() => ({ loading: false }));
            return {} as ApiResponse<User[]>;
        }
    },
    GetTeacherRoom: async (idRoom: number): Promise<ApiResponse<User[]>> => {
        try {
            const storeUsers = get().teacher;
            const lastId = get().lastFetchedRoomId;

            const shouldFetch =
                idRoom !== lastId ||
                !Array.isArray(storeUsers) ||
                storeUsers.length === 0;

            let dataUsers: User[];
            let dataReponse: ApiResponse<User[]>;

            if (shouldFetch) {
                set(() => ({ loading: true }));
                dataReponse = await UserService.GetTeacherRoom(idRoom);
                dataUsers = dataReponse.data;
                get().setLastFetchedRoomId(idRoom);
            } else {
                dataReponse = {} as ApiResponse<User[]>;
                dataReponse.data = storeUsers;
                dataUsers = storeUsers;
            }

            set((state) => {
                const currentPageToAddRoomIndex = state.currentPageToAddRoom;
                const currentPageToAddRoomStart = currentPageToAddRoomIndex * 10;
                const currentPageToAddRoomEnd = (currentPageToAddRoomIndex + 1) * 10;

                const currentDisplayUsersToAddRoom = dataUsers.slice(
                    currentPageToAddRoomStart,
                    currentPageToAddRoomEnd
                );

                return {
                    ...state,
                    teacher: dataUsers,
                    changesUsers: dataUsers,
                    currentDisplayUsers: currentDisplayUsersToAddRoom,
                    loading: false,
                    pageCount: Math.ceil(dataUsers.length / 10),
                };
            });

            return dataReponse;
        } catch (error) {
            console.error("Error during login:", error);
            set(() => ({ loading: false }));
            return {} as ApiResponse<User[]>;
        }
    },
    AddTeacherRoom: async (
        data: QuitAddTeacherRoom
    ): Promise<ApiResponse<string>> => {
        try {
            const dataResposne = await UserService.AddTeacherRoom(data);

            if (dataResposne.result) {
                set((state) => {
                    const currentToAddRoom = Array.isArray(state.changesUsersToAddRoom)
                        ? state.changesUsersToAddRoom
                        : [];

                    const currentTeachers = Array.isArray(state.teacher)
                        ? state.teacher
                        : [];

                    // Filtramos los profesores agregados desde "para agregar"
                    const updatedToAdd = currentToAddRoom.filter(
                        (user) => !data.idTeachers.includes(user.id)
                    );

                    // Obtenemos los usuarios que fueron agregados
                    const addedTeachers = currentToAddRoom.filter((user) =>
                        data.idTeachers.includes(user.id)
                    );

                    // Si `currentTeachers` está vacío, no agregamos `addedTeachers`
                    const updatedInRoom =
                        currentTeachers.length > 0
                            ? [...currentTeachers, ...addedTeachers]
                            : currentTeachers;

                    return {
                        ...state,
                        // Actualizamos los profesores para agregar
                        teacherToAddRoom: updatedToAdd,
                        changesUsersToAddRoom: updatedToAdd,
                        currentDisplayUsersToAddRoom: updatedToAdd.slice(
                            state.currentPageToAddRoom * 10,
                            (state.currentPageToAddRoom + 1) * 10
                        ),
                        pageCountToAddRoom: Math.ceil(updatedToAdd.length / 10),

                        // Actualizamos los profesores que ya están en la sala
                        teacher: updatedInRoom,
                        changesUsers: updatedInRoom,
                        currentDisplayUsers: updatedInRoom.slice(
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
            return {} as ApiResponse<string>;
        }
    },
    QuitTeacherRoom: async (
        data: QuitAddTeacherRoom
    ): Promise<ApiResponse<string>> => {
        try {
            const dataResposne = await UserService.QuitTeacherRoom(data);

            if (dataResposne.result) {
                set((state) => {
                    const currentChangesUsers = Array.isArray(state.changesUsers)
                        ? state.changesUsers
                        : [];

                    const currentToAddRoom = Array.isArray(state.teacherToAddRoom)
                        ? state.teacherToAddRoom
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
                        (user) => !data.idTeachers.includes(user.id)
                    );

                    const removedTeachers = currentChangesUsers.filter((user) =>
                        data.idTeachers.includes(user.id)
                    );

                    const updatedToAdd = [...currentToAddRoom, ...removedTeachers];

                    return {
                        ...state,
                        teacher: updatedInRoom,
                        changesUsers: updatedInRoom,
                        currentDisplayUsers: updatedInRoom.slice(
                            state.currentPage * 10,
                            (state.currentPage + 1) * 10
                        ),
                        ...(currentToAddRoom.length > 0 && {
                            teacherToAddRoom: updatedToAdd,
                            changesUsersToAddRoom: updatedToAdd,
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
            return {} as ApiResponse<string>;
        }
    },

    // Teacher to add room
});

export const useTeacherRoomStore = create<UserState>()(creatorUser);
