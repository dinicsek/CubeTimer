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
    const [time, setTime] = useState(0);
    const [isInspectionEnabled, setIsInspectionEnabled] = useState(false);
    const token = localStorage.getItem("token");
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
                        // fetch("http://localhost:5123/Solves", {
                        //     method: 'POST',
                        //     headers: {
                        //         "Content-Type": "application/json",
                        //         "Authorization": "Bearer " + localStorage.getItem("token")
                        //     },
                        //     body: JSON.stringify(time),
                        // })
                        document.dispatchEvent(new Event("regenerateScramble"));
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
            intervalId = setInterval(() => setTime(time + 1), 10);
        }
        return () => clearInterval(intervalId);
    }, [state, time]);
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

export default Stopwatch;
