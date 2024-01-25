import React, { createContext, useState } from "react";

export const SolveContext = createContext({
    solve: { Time: 0, Modifier: null, Scramble: "", Session: null, Cube: 1 },
    setSolve: (value: { Time: number, Modifier: any, Scramble: string, Session: any, Cube: number }) => {
    }
});

export const ContextProvider = ({ children }: { children: React.ReactNode }) => {
    const [solve, setSolve] = useState({
        Time: 0,
        Modifier: null,
        Scramble: "",
        Session: null,
        Cube: 1
    });

    return (
        <SolveContext.Provider value={{ solve, setSolve }}>
            {children}
        </SolveContext.Provider>
    );
};
