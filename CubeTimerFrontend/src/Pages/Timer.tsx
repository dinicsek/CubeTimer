import Stopwatch, { getTime } from "../Components/Stopwatch.tsx";
import React, { useContext, useEffect, useState } from "react";
import { generateScramble } from "../Components/Utils/generateScramble.ts";
import { CubeEvent } from "../Enums/CubeEvent.ts";
import Settings from "../Components/Settings.tsx";
import { SettingsContext } from "../Contexts/SettingsContext.tsx";

function Timer() {
    const [settings, setSettings] = useState(useContext(SettingsContext));
    const [scramble, setScramble] = useState(generateScramble(settings.event, settings.length));
    let displayEvent = "333";
    switch (settings.event) {
        case CubeEvent.OneByOne:
            displayEvent = "111";
            break;
        case CubeEvent.TwoByTwo:
            displayEvent = "222";
            break;
        case CubeEvent.ThreeByThree:
            displayEvent = "333";
            break;
        case CubeEvent.FourByFour:
            displayEvent = "444";
            break;
        case CubeEvent.FiveByFive:
            displayEvent = "555";
            break;
        case CubeEvent.Pyraminx:
            displayEvent = "pyram";
    }

    useEffect(() => {
        document.addEventListener("regenerateScramble", () => {
            fetch("http://localhost:5123/Solves", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${localStorage.getItem("token")}`
                },
                body: JSON.stringify({
                    "cubeId": 1,
                    "scramble": scramble,
                    "sessionId": 1,
                    "time": getTime()
                })
            }).then((res) => {
                console.log(res);
            });
            console.log(getTime());
            setScramble(generateScramble(settings.event, settings.length));
        });
    });
    return (
        <div className="bg-blue-600 w-screen h-screen overflow-hidden">
            <div className="absolute top-4 right-4 size-[30px]">
                <Settings />
            </div>
            <div className="flex justify-center items-center flex-col mt-6">
                <p className="text-3xl text-center text-balance">{scramble}</p>
            </div>
            <div className="text-8xl absolute top-[45%] left-1/2 -translate-y-1/2 -translate-x-1/2 ">
                <Stopwatch />
            </div>
            <div
                className=" max-sm:right-1/2 max-sm:translate-x-1/2 rounded-[20px] absolute right-4 bottom-4 border-solid border-2 border-white/[0.2] outline-none shadow-white shadow-sm backdrop-blur-sm">
                {/* @ts-expect-error*/}
                <scramble-display event={displayEvent} scramble={scramble} />
            </div>
        </div>
    );
}

export default Timer;
