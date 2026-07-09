import { Login } from "../pages/Login";
import { ForgotPassword } from "../components/login/FogotPassword";
import { RouteObject } from "react-router-dom";

export const AuthRoutes: RouteObject[] = [
  {
    path: "/auth/login",
    element: <Login />,
  },
  {
    path: "/auth/forgot-password",
    element: <ForgotPassword />,
  },
];
