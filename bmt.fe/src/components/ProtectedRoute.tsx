import React, { ReactNode, ReactElement } from "react";
import { Navigate } from "react-router-dom";
import AuthService from "../services/AuthService";

interface ProtectedRouteProps {
  children: ReactNode;
}

const ProtectedRoute = ({ children }: ProtectedRouteProps): ReactElement | null => {
  const token = AuthService.getAccessToken();
  return token ? <>{children}</> : <Navigate to="/login" replace />;
};

export default ProtectedRoute;
