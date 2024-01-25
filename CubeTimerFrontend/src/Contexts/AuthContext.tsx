import React, { createContext, useState } from "react";


export const AuthContext = createContext({
    authenticated: false,
    setAuthenticated: (value: boolean) => {
    }
});

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
    const [authenticated, setAuthenticated] = useState(false);

    return (
        <AuthContext.Provider value={{ authenticated, setAuthenticated }}>
            {children}
        </AuthContext.Provider>
    );
};
