import { create, StateCreator } from "zustand";
import { ApiResponse } from "../../../interfaces/ApiResponse";
import { User } from "../../../interfaces/Auth/User";
import UserService from "../../../services/Users/Users.Service";
import { searchUser } from "../../../util/User/SearchUser";

interface UserState {
    // Properties
    loading: boolean;
    users: User[];
    imutableUsers: User[];
    changesUsers: User[];
    currentDisplayUsers: User[];
    currentPage: number;
    pageCount: number;
    searchTerm: string;

    // Actions
    setPageCount: (count: number) => void;
    setSearchTerm: (searchTerm: string) => void;
    setCurrentDisplayUsers: (users: User[], currentPage: number) => void;
    setChangesUsers: (users: User[]) => void;
    setLoading: (statu: boolean) => void;
    getAllUsersAsync: (
        roles: string[],
        loadForce?: boolean
    ) => Promise<ApiResponse<User[]>>;
    onCreateOrUpdate: (user: User) => Promise<ApiResponse<string>>;
    onInactiveActive: (iduser: string) => Promise<ApiResponse<string>>;
}

const creatorUser: StateCreator<UserState> = (set, get) => ({
    searchTerm: "",
    loading: false,
    currentPage: 0,
    pageCount: 0,
    imutableUsers: {} as User[],
    users: {} as User[],
    roles: {} as string[],
    changesUsers: {} as User[],
    currentDisplayUsers: {} as User[],

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

    getAllUsersAsync: async (
        roles: string[],
        loadForce?: boolean
    ): Promise<ApiResponse<User[]>> => {
        try {
            let dataUsers: User[];
            let imutableUsers: User[];
            let dataReponse: ApiResponse<User[]>;
            const storeUsers = get().users;

            if (loadForce || !Array.isArray(storeUsers) || storeUsers.length === 0) {
                set(() => ({ loading: true }));
                dataReponse = await UserService.GetAllUsersAsync();
                imutableUsers = dataReponse.data;
                dataUsers = dataReponse.data;
            } else {
                dataReponse = {} as ApiResponse<User[]>;
                dataReponse.data = storeUsers;
                imutableUsers = storeUsers;
                dataUsers = storeUsers;
            }

            dataUsers = dataUsers.filter(
                (user) =>
                    Array.isArray(user.roles) &&
                    user.roles.some((role) => roles.includes(role))
            );

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
                const updatedUsers = state.imutableUsers.map((user) =>
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
                    imutableUsers: updatedUsers,
                    changesUsers: updatedUsers,
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
    onCreateOrUpdate: async (user: User): Promise<ApiResponse<string>> => {
        try {
            set(() => ({ loading: true }));

            // Realizar la llamada a la API
            const data = await UserService.CreateOrUpdate(user);

            // Actualizar el estado del store
            set((state) => {
                if (!Array.isArray(state.users) || state.users.length === 0) {
                    return { ...state, loading: false };
                }

                const currentPageIndex = state.currentPage;

                if (state.users.length > 0) {
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
                    // Calcular la posición de los usuarios a mostrar según la página actual
                    const currentPageStart = currentPageIndex * 10;
                    const currentPageEnd = (currentPageIndex + 1) * 10;
                    const currentPageUsers = updatedUsers.slice(
                        currentPageStart,
                        currentPageEnd
                    );
                    // Actualizar las listas de usuarios
                    return {
                        ...state,
                        users: updatedUsers,
                        imutableUsers,
                        changesUsers: updatedUsers,
                        currentDisplayUsers: currentPageUsers,
                        loading: false,
                    };
                }
                state.loading = false;
                return state;
            });

            return data;
        } catch (error) {
            console.error("Error during login:", error);
            set(() => ({ loading: false }));
            return {} as ApiResponse<string>;
        }
    },
});

export const useUserParentsStore = create<UserState>()(creatorUser);
