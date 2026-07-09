import { Navigate } from "react-router-dom";
import { useAuthStore } from "../hooks/store/Auth.store";

export const RoleGuard = ({
  allowedRoles,
  children,
}: {
  allowedRoles: string[];
  children: React.ReactNode;
}) => {
  const user = useAuthStore((state) => state.UserLoggued);

  if (!user || !user.roles) {
    // Si no hay user, podrías redirigir a login
    return <Navigate to="/login" replace />;
  }

  // Verificar si el usuario tiene al menos uno de los roles permitidos
  const hasAccess = user.roles.some((role) => allowedRoles.includes(role));

  if (!hasAccess) {
    return <Navigate to="/not-authorized" replace />;
  }

  return <>{children}</>;
};
