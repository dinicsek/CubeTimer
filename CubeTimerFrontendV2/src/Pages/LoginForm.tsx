import {
    Anchor,
    Box,
    Button,
    Center,
    getGradient,
    Group,
    PasswordInput,
    rem,
    TextInput,
    Title,
    useMantineTheme
} from "@mantine/core";
import { useForm } from "@mantine/form";
import { Link } from "react-router-dom";
import DottedBg from "../Components/DottedBg";
import { useContext } from "react";
import { AuthContext } from "../Contexts/AuthContext.tsx";

const LoginForm = () => {
    const auth = useContext(AuthContext);
    const form = useForm(
        {
            initialValues:
                {
                    email: "",
                    password: ""
                }
        });
    const theme = useMantineTheme();
    const bg = getGradient({ deg: 37, from: "grape.8", to: "violet.9" }, theme);
    const submit = () => {
        console.log(form.values);
        fetch("http://localhost:5123/Auth/login", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(form.values),
        }).then((res) => {
            if (res.ok) {
                return res.json();
            }
            throw new Error("Something went wrong");
        }).then((data) => {
            localStorage.setItem("token", data.token);

            auth.setAuthenticated(true);
        }).catch((err) => {
            console.log(err);
            return err;
        });
    };
    return (
        <Box h={"100vh"} bg={bg} style={{ overflow: "hidden" }}>
            <Box visibleFrom="md">
                <DottedBg density="40" sizeX="20rem" sizeY="20rem" opacity="0.5" />
            </Box>
            <Center h={"100vh"}>
                <Box>
                    <Title size={rem(70)} mb={"20px"} ta={"center"} c={"gray.0"}>Sharp Timer</Title>
                    <Box style={{
                        zIndex: 20,
                        borderRadius: "20px",
                        padding: "1.25rem",
                        borderWidth: "2px",
                        borderColor: "white",
                        borderStyle: "solid",
                        outline: "none",
                        boxShadow: "0 0 10px rgba(255,255,255,0.1)",
                        backgroundColor: "rgba(255,255,255,0.1)",
                    }}>
                        <Title c={"white"} order={2} ta={"center"}>Login</Title>
                        <form onSubmit={submit}>
                            <TextInput
                                c={"white"}
                                placeholder="Your email"
                                radius="sm"
                                label="Email"
                                type="email"
                                mb={5}
                                required={true} {...form.getInputProps("email")}
                            />
                            <PasswordInput
                                c={"white"}
                                placeholder="Your password"
                                radius="sm"
                                label="Password"
                                mb={10}
                                required={true} {...form.getInputProps("password")}
                            />
                        </form>
                        <Group justify="space-between">
                            <Button type={"submit"} mr={5} onClick={submit}>Login</Button>
                            <Anchor component={Link}
                                c={"gray.3"}
                                to="/Register">
                                Dont have an account?
                            </Anchor>
                        </Group>
                    </Box>
                </Box>
            </Center>
            <Box style={{
                position: "absolute",
                bottom: 290,
                right: 450,
            }}
            visibleFrom="md">
                <DottedBg density="40" sizeX="448px" sizeY="18rem" opacity="0.5" />
            </Box>
        </Box>
    );
};

export default LoginForm;
