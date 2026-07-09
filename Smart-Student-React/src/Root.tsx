import { Navigate, Outlet, useLocation } from "react-router-dom";
import { RouteTracker } from "./routes/RouteTracker";
import { useAuthStore } from "./hooks/store/Auth.store";

export const Root = () => {
  const { pathname } = useLocation();
  const userLoggued = useAuthStore((state) => state.UserLoggued);
  const roles = userLoggued?.roles || [];

  const getRedirectPath = () => {
    if (roles.includes("Admin") || roles.includes("Coordinador")) {
      return "/home";
    }
    if (roles.includes("Profesor")) {
      return "/rooms";
    }
    return "/home";
  };

  // Cuando es root
  if (pathname === "/") {
    const redirectPath = getRedirectPath();
    return <Navigate to={redirectPath} replace />;
  }

  return (
    <main>
      <Outlet />
      <RouteTracker />
    </main>
  );
};
