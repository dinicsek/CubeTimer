import React, { useContext, useEffect, useRef, useState } from "react";
import { Box, rem, Title } from "@mantine/core";
import { SettingsContext } from "../Contexts/SettingsContext.tsx";

enum TimerState {
    Stopped,
    Inspection,
    Ready,
    Running
}

const Stopwatch = () => {
    const [state, _setState] = useState<TimerState>(TimerState.Stopped);
    const stateRef = useRef(state);
    const [startTime, setStartTime] = useState<number>(0);
    const [time, setTime] = useState(0);
    const setting = useContext(SettingsContext);
    const setState = (newState: TimerState) => {
        stateRef.current = newState;
        _setState(newState);
    };
    // on initial load
    useEffect(() => {
        const time = document.getElementById("time");
        // KEYDOWN
        window.addEventListener("keydown", (event) => {
            if (event.key === " ") {
                switch (stateRef.current) {
                    case TimerState.Stopped:
                        reset();
                        time.style.color = "red";
                        break;
                    case TimerState.Ready:
                        time.style.color = "green";
                        break;
                    case TimerState.Inspection:
                        setState(TimerState.Running);
                        break;
                    case TimerState.Running:
                        // document.dispatchEvent(new Event("regenerateScramble"));
                        setState(TimerState.Stopped);
                        time.style.color = "black";
                        break;
                }
            }
        });
        // KEYUP
        window.addEventListener("keyup", () => {
            switch (stateRef.current) {
                case TimerState.Stopped:
                    time.style.color = "black";
                    break;
                case TimerState.Inspection:
                    time.style.color = "black";
                    setState(TimerState.Running);
                    break;
                case TimerState.Ready:
                    if (setting.inspectionEnabled)
                        setState(TimerState.Inspection);
                    else {
                        setState(TimerState.Running);
                        time.style.color = "black";
                    }
                    break;
            }
        });
    });


    useEffect(() => {
        let intervalId: number;
        if (state === TimerState.Running) {
            if (startTime === 0) {
                setStartTime(Date.now());
            }
            intervalId = setInterval(() => {
                if (state === TimerState.Running) {
                    setTime(Math.floor((Date.now() - startTime) / 10));
                }
            }, 10);
        } else {
            setStartTime(0);
        }
        return () => clearInterval(intervalId);
    }, [state, startTime]);
    // Minutes calculation
    const minutes = Math.floor((time % 360000) / 6000);

    // Seconds calculation
    const seconds = Math.floor((time % 6000) / 100);

    // Milliseconds calculation
    const milliseconds = time % 100;

    const reset = () => {
        setTime(0);
    };
    return (
        <>
            <Box className="stopwatch-container">
                <Title size={rem(70)} className="stopwatch-time" id="time">
                    {minutes.toString().padStart(2, "0")}:
                    {seconds.toString().padStart(2, "0")}:
                    {milliseconds.toString().padStart(2, "0")}
                </Title>
            </Box>

        </>

    );
};
export const getTime = () => {
    let time = document.getElementById("time")!.innerHTML;
    time = time.replace(":", "");
    return +time;
};
export default Stopwatch;
