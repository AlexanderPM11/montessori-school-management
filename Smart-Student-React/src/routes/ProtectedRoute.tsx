import { Navigate } from "react-router-dom";
import { useAuthStore } from "../hooks/store/Auth.store";
import { NavBar } from '../components/NavBar';

export const ProtectedRoute = () => {
  const isLoggued = useAuthStore((state) => state.isLoggued);

  if (!isLoggued) {
    return <Navigate to="/auth/login" />;
  }

  return <NavBar />;

}
