import React, { useEffect, useRef, useState } from "react";

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
    const [isInspectionEnabled, setIsInspectionEnabled] = useState(false);
    const setState = (newState: TimerState) => {
        stateRef.current = newState;
        _setState(newState);
    };
    // on initial load
    useEffect(() => {
        const time = document.getElementById("time");
        window.addEventListener("keydown", (event) => {
            if (event.key === " ") {
                switch (stateRef.current) {
                    case TimerState.Stopped:
                        reset();
                        setState(TimerState.Ready);
                        time.style.color = "red";
                        break;
                    case TimerState.Inspection:
                        time.style.color = "green";
                        break;
                    case TimerState.Running:
                        // document.dispatchEvent(new Event("regenerateScramble"));
                        setState(TimerState.Stopped);
                        time.style.color = "black";
                        break;
                }
            }
        });
        window.addEventListener("keyup", () => {
            switch (stateRef.current) {
                case TimerState.Stopped:
                    break;
                case TimerState.Inspection:
                    time.style.color = "black";
                    setState(TimerState.Running);
                    break;
                case TimerState.Ready:
                    if (isInspectionEnabled)
                        setState(TimerState.Inspection);
                    else {
                        setState(TimerState.Running);
                        time.style.color = "black";
                    }
                    break;
            }
        });
    }, []);

    useEffect(() => {
        let intervalId;
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
            <div className="stopwatch-container">
                <p className="stopwatch-time" id="time">
                    {minutes.toString().padStart(2, "0")}:
                    {seconds.toString().padStart(2, "0")}:
                    {milliseconds.toString().padStart(2, "0")}
                </p>
            </div>

        </>

    );
};
export const getTime = () => {
    let time = document.getElementById("time")?.innerHTML;
    time = time.replace(":", "");
    return +time;
};
export default Stopwatch;
