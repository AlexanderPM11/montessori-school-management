import { create, StateCreator } from 'zustand'
import { ApiResponse } from '../../../interfaces/ApiResponse';
import UserService from '../../../services/Users/Users.Service';


interface UserState {

    // Properties
    loading: boolean;


    // Actions
    onImportUsers: (file: File) => Promise<ApiResponse<string>>;
    getAllRolesAsync: () => Promise<ApiResponse<string[]>>;

}


const creatorUserGeneral: StateCreator<UserState> = (set) => ({

    loading: false,

    roles: {} as string[],
    getAllRolesAsync: async (): Promise<ApiResponse<string[]>> => {

        try {
            set(() => ({ loading: true }));
            const data = await UserService.GetAllRolesAsync();

            set((state) => {
                state.loading = true;

                return {
                    ...state,
                    roles: data.data,
                    loading: false,
                };

            });
            return data;

        } catch (error) {

            console.error("Error during login:", error);
            set(() => ({ loading: false }));
            return {} as ApiResponse<string[]>
        }


    },
    onImportUsers: async (file: File): Promise<ApiResponse<string>> => {
        try {

            // Realizar la llamada a la API
            const data = await UserService.ImportUsers(file);
            return data;

        } catch (error) {
            console.error("Error during login:", error);
            set(() => ({ loading: false }));
            return {} as ApiResponse<string>;
        }
    },




})



export const useUserGeneralStore = create<UserState>()(
    creatorUserGeneral
);
