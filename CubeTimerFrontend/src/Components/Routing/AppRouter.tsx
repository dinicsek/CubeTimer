import { Route, Routes } from "react-router-dom";
import LoginForm from "../../Pages/LoginForm.tsx";
import RegisterForm from "../../Pages/RegisterForm.tsx";
import TimerPage from "../../Pages/TimerPage.tsx";
import { UnAuthenticatedGuard } from "./Guards/UnAuthenticatedGuard.tsx";
import { AuthenticatedGuard } from "./Guards/AuthenticatedGuard.tsx";


export const AppRouter = () => {
    return (
        <Routes>
            <Route element={<UnAuthenticatedGuard />}>
                <Route path={"/Register"} element={<RegisterForm />} />
                <Route path={"/Login"} element={<LoginForm />} />
            </Route>
            <Route element={<AuthenticatedGuard />}>
                <Route path={"/"} element={
                    <TimerPage />
                } />
            </Route>
        </Routes>
    );
};
