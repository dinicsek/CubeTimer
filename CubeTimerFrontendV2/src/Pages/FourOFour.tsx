import { Box, Button, Center, Text, Title } from "@mantine/core";
import { Link } from "react-router-dom";

export const FourOFour = () => {
    return (
        <Center h={"100vh"}>
            <Box ta="center">
                <Title>404 </Title>
                <Title mb={"20px"}> Page Not Found</Title>
                <Text mb={"10px"}>The page you just tried to reach does not exist.</Text>
                <Button size={"md"} component={Link} to="/" variant="outline">Take me back!</Button>
            </Box>
        </Center>
    );
};
