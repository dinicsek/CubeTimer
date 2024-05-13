import { Link, useNavigate } from "react-router-dom";
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
import DottedBg from "../Components/DottedBg.tsx";

const RegisterForm = () => {
    const theme = useMantineTheme();
    const bg = getGradient({ deg: 180 - 37, from: "grape.8", to: "violet.9" }, theme);

    const form = useForm({
        initialValues: {
            email: "",
            firstName: "",
            lastName: "",
            username: "",
            password: "",
            passwordConfirmation: ""
        }
    });

    const navigate = useNavigate();

    const submit = () => {
        if (form.values.password !== form.values.passwordConfirmation) {
            alert("Passwords do not match!");
            return false;
        }
        fetch("http://localhost:5123/Auth/register", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(form.values),
        }).then((res) => {
            if (res.ok) {
                navigate("/Login");
            } else {
                throw new Error("Something went wrong");
            }
        }).catch((err) => {
            console.log(err);
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
                        <Title c={"white"} order={2} ta={"center"}>Register</Title>
                        <form onSubmit={submit}>
                            <TextInput c={"white"}
                                placeholder="Your email"
                                radius="sm"
                                label="Email"
                                type="email"
                                mb={5}
                                required={true} {...form.getInputProps("email")} />
                            <TextInput c={"white"}
                                placeholder="JohnDoe123"
                                radius="sm"
                                label="Username"
                                mb={5}
                                required={true} {...form.getInputProps("username")} />
                            <Box style={{
                                position: "relative",
                                display: "flex",
                                flexDirection: "row",
                            }}>
                                <TextInput c={"white"}
                                    placeholder="John"
                                    radius="sm"
                                    label="First name"
                                    mb={5}
                                    mr={3}
                                    flex={1}
                                    required={true} {...form.getInputProps("firstName")} />
                                <TextInput c={"white"}
                                    placeholder="Doe"
                                    radius="sm"
                                    flex={1}
                                    label="Last name"
                                    mb={5}
                                    required={true} {...form.getInputProps("lastName")} />
                            </Box>
                            <Box style={{
                                position: "relative",
                                display: "flex",
                                flexDirection: "row",
                            }}>
                                <PasswordInput c={"white"}
                                    w={"50%"}
                                    placeholder="Your password"
                                    radius="sm"
                                    label="Password"
                                    mb={10}
                                    flex={1}
                                    mr={3}
                                    required={true} {...form.getInputProps("password")} />
                                <PasswordInput c={"white"}
                                    w={"50%"}
                                    placeholder="Your password again"
                                    radius="sm"
                                    label="Confirm password"
                                    flex={1}
                                    mb={10}
                                    required={true} {...form.getInputProps("passwordConfirmation")} />
                            </Box>
                        </form>
                        <Group justify="space-between">
                            <Button pos={"inherit"} onClick={submit} type={"submit"} ta={"left"}
                                justify="left">Register</Button>
                            <Anchor ta={"right"}
                                component={Link}
                                c={"gray.3"}
                                to="/Login">
                                Already have an account?
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

export default RegisterForm;
