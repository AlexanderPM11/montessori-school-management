import { Link } from "react-router-dom";
import { useAuthStore } from "../hooks/store/Auth.store";

export const ForbiddenPage = () => {
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
  return (
    <div className="flex flex-col items-center justify-center min-h-screen bg-gray-100 px-4">
      <h1 className="text-6xl font-bold text-red-600">403</h1>
      <p className="mt-4 text-2xl font-semibold text-gray-800">
        Acceso denegado
      </p>
      <p className="mt-2 text-gray-500 text-center max-w-md">
        No tienes permisos para acceder a esta página. Si crees que es un error,
        por favor contacta al administrador.
      </p>
      <Link
        to={getRedirectPath()}
        className="mt-6 inline-block bg-red-600 text-white px-6 py-3 rounded-md hover:bg-red-700 transition"
      >
        Volver al inicio
      </Link>
    </div>
  );
};
