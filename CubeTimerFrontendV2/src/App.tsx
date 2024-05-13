import React, { Suspense, useContext, useEffect } from "react";
import ReactDOM from "react-dom/client";
import { AuthContext, AuthProvider } from "./Contexts/AuthContext.tsx";
import { BrowserRouter } from "react-router-dom";
import { AppRouter } from "./Components/Routing/AppRouter.tsx";
import { SettingsContextProvider } from "./Contexts/SettingsContext.tsx";
import { MantineProvider } from "@mantine/core";
import "@mantine/core/styles.css";
import { FullScreenLoading } from "./Components/FullScreenLoading.tsx";


function App() {
    const auth = useContext(AuthContext);
    useEffect(() => {
        console.log(
            "   _____ _                   _______ _                     \n  / ____| |                 |__   __(_)                    \n | (___ | |__   __ _ _ __ _ __ | |   _ _ __ ___   ___ _ __ \n  \\___ \\| '_ \\ / _` | '__| '_ \\| |  | | '_ ` _ \\ / _ \\ '__|\n  ____) | | | | (_| | |  | |_) | |  | | | | | | |  __/ |   \n |_____/|_| |_|\\__,_|_|  | .__/|_|  |_|_| |_| |_|\\___|_|   \n                         | |                               \n"
        );
    }, []);
    useEffect(() => {
        const token = localStorage.getItem("token");

        console.log(token);

        if (token) {
            auth.setAuthenticated(true);
        }
    }, [auth]);
    return (
        <SettingsContextProvider>
            <MantineProvider>
                <Suspense fallback={<FullScreenLoading />}>
                    <BrowserRouter>
                        <AppRouter />
                    </BrowserRouter>
                </Suspense>
            </MantineProvider>
        </SettingsContextProvider>
    );
}

ReactDOM.createRoot(document.getElementById("root")!).render(<AuthProvider><App /></AuthProvider>);
