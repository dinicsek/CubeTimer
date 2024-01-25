import Stopwatch from "../Components/Stopwatch.tsx";
import React, { useEffect, useState } from "react";
import { ScrambleLengthSettingContext } from "../Contexts/ScrambleLengthSettingContext.tsx";
import { generateScramble } from "../Components/Utils/generateScramble.ts";
import { CubeEvent } from "../Enums/CubeEvent.ts";

function Timer() {
    const length = React.useContext(ScrambleLengthSettingContext);
    const [scramble, setScramble] = useState(generateScramble(CubeEvent.FourByFour, length));

    useEffect(() => {
        document.addEventListener("regenerateScramble", () => {
            setScramble(generateScramble(CubeEvent.FourByFour, length));
        });
    }, []);
    return (
        <div className="bg-blue-600 w-screen h-screen overflow-hidden">
            <div className="flex justify-center items-center flex-col m-6">
                <p className="text-3xl text-center text-balance">{scramble}</p>
            </div>
            <div className="text-8xl absolute top-[45%] left-1/2 -translate-y-1/2 -translate-x-1/2">
                <Stopwatch />
            </div>
            <div
                className=" max-sm:right-1/2 max-sm:translate-x-1/2 rounded-[20px] absolute right-4 bottom-4 border-solid border-2 border-white/[0.2] outline-none shadow-white shadow-sm backdrop-blur-sm">
                {/* @ts-expect-error*/}
                <scramble-display event="444" scramble={scramble} />
            </div>
        </div>
    );
}

export default Timer;
