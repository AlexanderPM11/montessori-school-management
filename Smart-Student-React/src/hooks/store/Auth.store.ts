import { create, StateCreator } from 'zustand'
import { ApiResponse } from '../../interfaces/ApiResponse';
import { User } from '../../interfaces/Auth/User';
import AuthService from '../../services/Auth/Auth.Services';
import { LoginInterface } from '../../interfaces/Auth/Login';
import { persist } from 'zustand/middleware';
import { ChangePasswordInterface } from '../../interfaces/Auth/ChangePasswordInterface';
import UserService from '../../services/Users/Users.Service';
import { setItemDB } from '../../util/indexedDB';

interface AuthState {

    // Properties
    UserLoggued: User;
    loading: boolean;
    isLoggued: boolean;
    tokenDate: Date | null;

    // Actions
    setLoading: (statu: boolean) => void;
    setRefreshToken: (token: string, refreshToken: string) => Promise<void>;
    login: (credentials: LoginInterface) => Promise<ApiResponse<User>>;
    logout: () => void;
    forgotPassword: (email: string) => Promise<ApiResponse<string>>;
    changePassword: (values: ChangePasswordInterface) => Promise<ApiResponse<string>>;
    onUpdateUserLogin: (user: User) => Promise<ApiResponse<string>>;


}


const creatorAuth: StateCreator<AuthState> = (set) => ({

    UserLoggued: {} as User,
    loading: false,
    isLoggued: false,
    tokenDate: null,

    setLoading: (statu: boolean) => set(() => ({
        loading: statu
    })),

    setRefreshToken: async (token: string, refreshToken: string) => {
        try {

            set(() => ({ loading: true }));


            set((state) => ({
                ...state,
                UserLoggued: {
                    ...state.UserLoggued,
                    token: token,
                    refreshToken: refreshToken
                },
                tokenDate: new Date(),
                loading: false
            }));

        } catch (error) {
            console.error("Error while refreshing token:", error);
            set(() => ({
                UserLoggued: {} as User,
                loading: false,
                tokenDate: null,
                isLoggued: false
            }));
        }
    },
    logout: () => {
        set((state) => ({
            ...state,
            UserLoggued: {} as User,
            loading: false,
            tokenDate: null,
            isLoggued: false,
        }));
    },
    login: async (credentials: LoginInterface) => {
        try {
            set(() => ({ loading: true }));
            // Realizar la llamada a la API
            const data = await AuthService.LoginAsync(credentials);
            // Asegurarse de que data.data no sea null
            const safeData = data.data ?? {};
            const { urlImage, ...userWithoutImage } = safeData;

            // Guardar urlImage en IndexedDB
            if (urlImage) {
                await setItemDB('user-image', urlImage);
            }
            // Actualizar el estado con los datos de la respuesta
            set((state) => ({
                ...state,
                UserLoggued: userWithoutImage as User,
                isLoggued: data.result,
                tokenDate: new Date(),
                loading: false
            }));
            return data;

        } catch (error) {
            console.error("Error during login:", error);
            set(() => ({
                UserLoggued: {} as User,
                isLoggued: false,
                tokenDate: null,
                loading: false
            }));
            throw error; // 
        }
    }
    ,
    forgotPassword: async (email: string) => {

        const data = await AuthService.ForgotPasswordAsync(email);
        return data;
    },
    changePassword: async (values: ChangePasswordInterface) => {

        const data = await AuthService.ChangePasswordAsync(values);
        return data;
    },
    onUpdateUserLogin: async (user: User): Promise<ApiResponse<string>> => {
        try {
            set(() => ({ loading: true }));

            const data = await UserService.CreateOrUpdate(user);


            return data;

        } catch (error) {
            console.error("Error during onCreateOrUpdate:", error);
            set(() => ({ loading: false }));
            return {} as ApiResponse<string>;
        }
    }

})



export const useAuthStore = create<AuthState>()(
    persist(
        creatorAuth,
        { name: 'position-storage' }
    )
);
