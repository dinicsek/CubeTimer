import { createContext, useState } from "react";
import { CubeEvent } from "../Enums/CubeEvent";

export const SettingsContext = createContext({
    length: 20,
    setLength: (value: number) => {
    },
    inspectionEnabled: false,
    setInspectionEnabled: (value: boolean) => {
    },
    event: CubeEvent.ThreeByThree,
    setEvent: (value: CubeEvent) => {
    },
    cubeType: "",
    setCubeType: (value: string) => {
    },
});

export const SettingsContextProvider = ({ children }: { children: React.ReactNode }) => {
    const [length, setLength] = useState(20);
    const [inspectionEnabled, setInspectionEnabled] = useState(false);
    const [event, setEvent] = useState(CubeEvent.ThreeByThree);
    const [cubeType, setCubeType] = useState("");

    return (
        <SettingsContext.Provider value={{
            length,
            setLength,
            inspectionEnabled,
            setInspectionEnabled,
            event,
            setEvent,
            cubeType,
            setCubeType
        }}>
            {children}
        </SettingsContext.Provider>
    );
};
