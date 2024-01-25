import { Navigate, Outlet } from "react-router-dom";
import { useContext } from "react";
import { AuthContext } from "../../../Contexts/AuthContext.tsx";

export const UnAuthenticatedGuard = ({ redirect = "/" }: { redirect?: string }) => {
    const isAuthenticated = useContext(AuthContext);

    return !isAuthenticated.authenticated ? <Outlet /> : <Navigate to={redirect} replace={true} />;
};
