import { Route, Routes } from "react-router-dom";
import { UnAuthenticatedGuard } from "./Guards/UnAuthenticatedGuard.tsx";
import { AuthenticatedGuard } from "./Guards/AuthenticatedGuard.tsx";
import { lazy, Suspense } from "react";
import { FullScreenLoading } from "../FullScreenLoading.tsx";
import { FourOFour } from "../../Pages/FourOFour.tsx";


export const AppRouter = () => {
    const RegisterForm = lazy(() => import("../../Pages/RegisterForm.tsx"));
    const LoginForm = lazy(() => import("../../Pages/LoginForm.tsx"));
    const TimerPage = lazy(() => import("../../Pages/TimerPage.tsx"));

    return (
        <Suspense fallback={<FullScreenLoading />}>
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
                <Route path={"*"} element={<FourOFour />} />
            </Routes>
        </Suspense>
    );
};
