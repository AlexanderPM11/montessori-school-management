import { useAuthStore } from "../hooks/store/Auth.store";


export const isTokenExpired = (): boolean => {

    const token = useAuthStore.getState().tokenDate;
    if (!token) return false;

    const tokenTime = new Date(token).getTime();
    const currentTime = Date.now();
    const fiftyFiveMinutesInMillis = 55 * 60 * 1000;

    return currentTime - tokenTime >= fiftyFiveMinutesInMillis;
};