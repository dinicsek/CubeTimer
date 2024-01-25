// Date: 07/04/21
import React, { useContext, useEffect } from "react";
import ReactDOM from "react-dom/client";
import { BrowserRouter } from "react-router-dom";
import { AppRouter } from "./Components/Routing/AppRouter.tsx";
import { AuthContext, AuthProvider } from "./Contexts/AuthContext.tsx";


function App() {
    const auth = useContext(AuthContext);

    useEffect(() => {
        console.log(
            "   _____ _                   _______ _                     \n  / ____| |                 |__   __(_)                    \n | (___ | |__   __ _ _ __ _ __ | |   _ _ __ ___   ___ _ __ \n  \\___ \\| '_ \\ / _` | '__| '_ \\| |  | | '_ ` _ \\ / _ \\ '__|\n  ____) | | | | (_| | |  | |_) | |  | | | | | | |  __/ |   \n |_____/|_| |_|\\__,_|_|  | .__/|_|  |_|_| |_| |_|\\___|_|   \n                         | |                               \n"
        );

        const token = localStorage.getItem("token");

        if (token) {
            auth.setAuthenticated(true);
        }
    }, []);
    return (
        <BrowserRouter>
            <AppRouter />
        </BrowserRouter>
    );
}

ReactDOM.createRoot(document.getElementById("root")!).render(
    <AuthProvider>
        <App />
    </AuthProvider>
);
