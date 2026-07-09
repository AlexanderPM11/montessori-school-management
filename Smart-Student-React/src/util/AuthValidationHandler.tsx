import { Navigate } from "react-router-dom";
import { useAuthStore } from "../hooks/store/Auth.store";

export const AuthValidationHandler = () => {
    const isLoggued = useAuthStore((state) => state.isLoggued);

    if (!isLoggued) {
        return <Navigate to="/auth/login" />;
    }
}