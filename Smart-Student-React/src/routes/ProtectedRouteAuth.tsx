import { Navigate, Outlet } from "react-router-dom";
import { useAuthStore } from "../hooks/store/Auth.store";

export const ProtectedRouteAuth = () => {
    const isLoggued = useAuthStore((state) => state.isLoggued);

    if (isLoggued) {
        return <Navigate to="/" />;
    }
    return <Outlet />;

}


