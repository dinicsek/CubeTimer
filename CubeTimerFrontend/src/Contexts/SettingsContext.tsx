import { createContext, useState } from "react";
import { CubeEvent } from "../Enums/CubeEvent";

export const ScrambleLength = createContext({
    length: 20,
    setLength: (value: number) => {
    }
});

export const ScrambleLengthProvider = ({ children }: { children: React.ReactNode }) => {
    const [length, setLength] = useState(20);

    return (
        <ScrambleLength.Provider value={{ length, setLength }}>
            {children}
        </ScrambleLength.Provider>
    );
};

export const InspectionEnabled = createContext({
    inspectionEnabled: false,
    setInspectionEnabled: (value: boolean) => {
    }

});

export const InspectionEnabledProvider = ({ children }: { children: React.ReactNode }) => {
    const [inspectionEnabled, setInspectionEnabled] = useState(false);

    return (
        <InspectionEnabled.Provider value={{ inspectionEnabled, setInspectionEnabled }}>
            {children}
        </InspectionEnabled.Provider>
    );
};

export const Event = createContext({
    event: CubeEvent.ThreeByThree,
    setEvent: (value: CubeEvent) => {
    }


});

export const EventProvider = ({ children }: { children: React.ReactNode }) => {
    const [event, setEvent] = useState(CubeEvent.ThreeByThree);

    return (
        <Event.Provider value={{ event, setEvent }}>
            {children}
        </Event.Provider>
    );
};

export const CubeType = createContext({
    cubeType: "",
    setCubeType: (value: string) => {
    }
});

export const CubeTypeProvider = ({ children }: { children: React.ReactNode }) => {
    const [cubeType, setCubeType] = useState("");

    return (
        <CubeType.Provider value={{ cubeType, setCubeType }}>
            {children}
        </CubeType.Provider>
    );
};
