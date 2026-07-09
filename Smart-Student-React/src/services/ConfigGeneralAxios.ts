import axios, { AxiosInstance } from 'axios';
import { useAuthStore } from '../hooks/store/Auth.store';
import { RefreshTokenConvert } from '../interfaces/Auth/RefreshToken';

const apiUrl = import.meta.env.VITE_API_URL || '';

const apiClient: AxiosInstance = axios.create({
    baseURL: `${apiUrl}/api/v1`,
    timeout: 120000,
});

// Interceptor para agregar el token de autenticación
apiClient.interceptors.request.use(
    async (config) => {
        const { UserLoggued } = useAuthStore.getState();
        if (UserLoggued?.token) {
            config.headers.set('Authorization', `Bearer ${UserLoggued.token}`);
        }
        return config;
    },
    (error) => Promise.reject(error)
);

// Interceptor de respuesta para manejar token expirado y refrescar
apiClient.interceptors.response.use(
    (response) => response,
    async (error) => {
        const originalRequest = error.config;
        const status = error?.response?.status;
        const isAuthError = status === 401 || status === 403 || status === undefined;

        if (isAuthError && !originalRequest._retry) {
            originalRequest._retry = true;

            const authStore = useAuthStore.getState();
            const { token, refreshToken, email } = authStore.UserLoggued || {};

            if (!refreshToken || !email) {
                authStore.logout();
                window.location.href = '/auth/login';
                return Promise.reject(error);
            }

            try {
                const refreshResponse = await axios.post(`${apiUrl}/api/v1/Account/RefreshToken`, {
                    Email: email,
                    Token: token,
                    RefreshToken: refreshToken,
                });

                const dataResponse = refreshResponse.data?.data;
                const newTokens = RefreshTokenConvert.toRefreshToken(dataResponse);

                // Guarda los nuevos tokens
                authStore.setRefreshToken(newTokens.token, newTokens.refreshToken);

                // Reintenta la petición original con nuevo token
                originalRequest.headers['Authorization'] = `Bearer ${newTokens.token}`;
                return apiClient(originalRequest);
            } catch (refreshError) {
                authStore.logout();
                window.location.href = '/auth/login';
                return Promise.reject(refreshError);
            }
        }

        return Promise.reject(error);
    }
);


export default apiClient;
