import { Box, Center, Loader } from "@mantine/core";

export const FullScreenLoading = () => {
    return (
        <Center style={{
            position: "fixed",
            top: 0,
            left: 0,
            height: "100%",
            zIndex: 1000,
            width: "100%",
        }}>
            <Box style={{
                display: "flex",
                flexDirection: "column",
                alignItems: "center",
            }}>
                <Loader type="bars" size="xl" color="black" />
            </Box>
        </Center>
    );
};
