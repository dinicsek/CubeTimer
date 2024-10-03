import React from "react";
import { Box, Center, getGradient, useMantineTheme } from "@mantine/core";
import Stopwatch from "../Components/Stopwatch.tsx";

function TimerPage() {
    const theme = useMantineTheme();
    const bg = getGradient({ deg: 37, from: "grape.8", to: "violet.9" }, theme);
    // const time = getTime();

    return (
        <Box h={"100vh"} bg={bg}>
            <Center h={"100vh"}>
                <Box>
                    <Stopwatch />
                </Box>
            </Center>
        </Box>
    );
}

export default TimerPage;
