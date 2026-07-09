import { useLocation } from "react-router-dom";
import { useEffect } from "react";
import { useAuthStore } from "../hooks/store/Auth.store";

export function RouteTracker() {
  const isLoggued = useAuthStore((state) => state.isLoggued);
  const location = useLocation();

  useEffect(() => {
    // No guardar rutas de login, registro, etc.
    if (isLoggued && !location.pathname.startsWith("/auth")) {
      localStorage.setItem(
        "lastVisitedUrl",
        location.pathname + location.search
      );
    }
  }, [location, isLoggued]);

  return null;
}
