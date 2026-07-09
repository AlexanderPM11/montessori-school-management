import { create, StateCreator } from "zustand";
import { ApiResponse } from "../../../interfaces/ApiResponse";
import { User } from "../../../interfaces/Auth/User";
import UserService from "../../../services/Users/Users.Service";
import { searchUser } from "../../../util/User/SearchUser";
import { FathersAndMothers } from "../../../interfaces/Auth/FathersAndMothers";

interface UserState {
    // Properties
    loading: boolean;
    users: User[];
    imutableUsers: User[];
    changesUsers: User[];
    currentDisplayUsers: User[];
    fathersAndMothers: FathersAndMothers;
    currentPage: number;
    pageCount: number;
    searchTerm: string;

    // Teacher to add room
    teachersToAddRoom: User[];

    // Actions
    setPageCount: (count: number) => void;
    setSearchTerm: (searchTerm: string) => void;
    setCurrentDisplayUsers: (users: User[], currentPage: number) => void;
    setChangesUsers: (users: User[]) => void;
    setLoading: (statu: boolean) => void;
    GetTeacherToAddRoom: (idRoom: number) => Promise<ApiResponse<User[]>>;
    getAllUsersAsync: (
        roles: string[],
        loadForce?: boolean
    ) => Promise<ApiResponse<User[]>>;
    getFathersAndMothers: () => Promise<FathersAndMothers>;
    onCreateOrUpdate: (
        user: User,
        roles: string[]
    ) => Promise<ApiResponse<string>>;
    onInactiveActive: (iduser: string) => Promise<ApiResponse<string>>;
}

const creatorUser: StateCreator<UserState> = (set, get) => ({
    searchTerm: "",
    loading: false,
    currentPage: 0,
    pageCount: 0,
    imutableUsers: {} as User[],
    users: {} as User[],
    teachersToAddRoom: {} as User[],
    changesUsers: {} as User[],
    currentDisplayUsers: {} as User[],
    fathersAndMothers: {} as FathersAndMothers,

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

    getFathersAndMothers: async (): Promise<FathersAndMothers> => {
        try {
            const storefathersAndMothers = get().fathersAndMothers;
            if (
                !Array.isArray(storefathersAndMothers) ||
                storefathersAndMothers.fathers.length === 0
            ) {
                const dataReponse = await UserService.GetFathersAndMothers();

                set((state) => {
                    return {
                        ...state,
                        fathersAndMothers: dataReponse.data,
                    };
                });

                return dataReponse.data;
            }
            return storefathersAndMothers;
        } catch (error) {
            console.error("Error during login:", error);
            set(() => ({ loading: false }));
            return {} as FathersAndMothers;
        }
    },

    getAllUsersAsync: async (
        roles: string[],
        loadForce?: boolean
    ): Promise<ApiResponse<User[]>> => {
        try {
            let dataUsers: User[];
            let dataReponse: ApiResponse<User[]>;
            const storeUsers = get().users;

            if (loadForce || !Array.isArray(storeUsers) || storeUsers.length === 0) {
                set(() => ({ loading: true }));
                dataReponse = await UserService.GetAllUsersAsync();
                dataUsers = dataReponse.data;
            } else {
                dataReponse = {} as ApiResponse<User[]>;
                dataReponse.data = storeUsers;
                dataUsers = storeUsers;
            }

            dataUsers = dataUsers.filter(
                (user) =>
                    Array.isArray(user.roles) &&
                    user.roles.some((role) => roles.includes(role))
            );
            const imutableUsers = dataUsers;

            set((state) => {
                const currentPageIndex = state.currentPage;
                const currentPageStart = currentPageIndex * 10;
                const currentPageEnd = (currentPageIndex + 1) * 10;

                const currentDisplayUsers = dataUsers.slice(
                    currentPageStart,
                    currentPageEnd
                );

                return {
                    ...state,
                    imutableUsers: imutableUsers,
                    users: dataUsers,
                    changesUsers: dataUsers,
                    currentDisplayUsers: currentDisplayUsers,
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

    onInactiveActive: async (iduser: string): Promise<ApiResponse<string>> => {
        try {
            set(() => ({ loading: true }));

            // Realizar la llamada a la API
            const data = await UserService.ActiveInactive(iduser);

            set((state) => {
                // Actualizar la lista de usuarios
                const updatedUsers = state.users.map((user) =>
                    user.id === iduser ? { ...user, estado: !user.estado } : user
                );

                // Filtrar los usuarios si hay un término de búsqueda
                let filteredUsers = updatedUsers;
                if (state.searchTerm.trim() !== "") {
                    filteredUsers = searchUser({
                        users: updatedUsers,
                        searchTerm: state.searchTerm,
                    });
                }

                // Calcular la cantidad de páginas y actualizar la lista de usuarios a mostrar
                const pageCount = Math.ceil(filteredUsers.length / 10);
                const currentDisplayUsers = filteredUsers.slice(
                    state.currentPage * 10,
                    (state.currentPage + 1) * 10
                );

                return {
                    ...state,
                    users: updatedUsers,
                    imutableUsers: updatedUsers, // Actualización correcta de la lista de usuarios inmutables
                    currentDisplayUsers, // Actualización correcta de la lista de usuarios a mostrar
                    pageCount, // Actualización de la cantidad de páginas
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
    onCreateOrUpdate: async (
        user: User,
        roles: string[]
    ): Promise<ApiResponse<string>> => {
        try {
            set(() => ({ loading: true }));

            const data = await UserService.CreateOrUpdate(user);

            set((state) => {
                if (!Array.isArray(state.users) || state.users.length === 0) {
                    return { ...state, loading: false };
                }
                const currentPageIndex = state.currentPage;


                const userExists = state.imutableUsers.some((u) => u.id === user.id);

                let imutableUsers: User[];
                let updatedUsers: User[];

                if (userExists) {
                    // Si el usuario existe, actualizarlo
                    updatedUsers = state.users.map((u) =>
                        u.id === user.id ? { ...u, ...user } : u
                    );
                    imutableUsers = state.imutableUsers.map((u) =>
                        u.id === user.id ? { ...u, ...user } : u
                    );
                } else {
                    // Si el usuario no existe, agregarlo
                    updatedUsers = [...state.users, user];
                    imutableUsers = [...state.imutableUsers, user];
                }

                // Filtrar por roles
                updatedUsers = updatedUsers.filter(
                    (u) =>
                        Array.isArray(u.roles) &&
                        u.roles.some((role) => roles.includes(role))
                );

                const currentPageStart = currentPageIndex * 10;
                const currentPageEnd = (currentPageIndex + 1) * 10;
                const currentDisplayUsers = updatedUsers.slice(
                    currentPageStart,
                    currentPageEnd
                );

                state.currentDisplayUsers = currentDisplayUsers;
                return {
                    ...state,
                    imutableUsers: imutableUsers,
                    users: updatedUsers,
                    changesUsers: updatedUsers,
                    currentDisplayUsers,
                    pageCount: Math.ceil(updatedUsers.length / 10),
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

    // Teacher to add room
    GetTeacherToAddRoom: async (idRoom: number): Promise<ApiResponse<User[]>> => {
        try {
            set(() => ({ loading: true }));
            const dataReponse = await UserService.GetTeacherToAddRoom(idRoom);

            set((state) => {
                return {
                    ...state,
                    teachersToAddRoom: dataReponse.data,
                    loading: false,
                };
            });
            return dataReponse;
        } catch (error) {
            console.error("Error during login:", error);
            set(() => ({ loading: false }));
            return {} as ApiResponse<User[]>;
        }
    },
});

export const useUserStore = create<UserState>()(creatorUser);
