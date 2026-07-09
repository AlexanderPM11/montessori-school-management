import { createBrowserRouter } from "react-router-dom";
import { Root } from "../Root";

import { ProtectedRoute } from "./ProtectedRoute";
import { DashboardRoutes } from "./DashboardRoutes";
import { ProtectedRouteAuth } from "./ProtectedRouteAuth";
import { AuthRoutes } from "./AuthRoutes";
import { NotFoundPage } from "../pages/NotFound";
import { ChangePassword } from "../components/login/ChangePassword";
import { ForbiddenPage } from "../components/ForbiddenPage";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <Root />,
    children: [
      // General Routes
      {
        path: "/auth/change-password",
        element: <ChangePassword />,
      },

      /// Dashboard Routes
      {
        path: "/",
        element: <ProtectedRoute />,
        children: DashboardRoutes,
      },

      /// Auth Routes

      {
        path: "/auth",
        element: <ProtectedRouteAuth />,
        children: AuthRoutes,
      },
      {
        path: "/not-authorized",
        element: <ForbiddenPage />,
      },

      /// Not Found Pages
      {
        path: "*",
        element: <NotFoundPage />,
      },
    ],
  },
]);
