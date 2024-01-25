// Date: 2023/01/03
// Desc: Login Form
import { EyeIcon, EyeSlashIcon, LockClosedIcon, UserCircleIcon } from "@heroicons/react/16/solid";
import { useState } from "react";
import "../index.css";
import "../assets/pexels-pixabay-417173.jpg";
import { Link } from "react-router-dom";

const LoginForm = () => {
    const [visible, setVisible] = useState(false);
    const [formData, setFormData] = useState({
        email: "",
        password: ""
    });

    function Submit() {
        fetch("http://localhost:5123/Auth/login", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(formData),
        }).then((res) => {
            if (res.ok) {
                return res.json();
            }
            throw new Error("Something went wrong");
        }).then((data) => {
            localStorage.setItem("token", data.token);
        }).catch((err) => {
            console.log(err);
        });
    }

    return (
        <>
            <div
                className="flex flex-col justify-center items-center h-screen w-screen bg-blue-600 font-mono m-0 p-0 box-border">
                <h1 className="text-7xl text-white m-6 font-bold"><span className="">Sharp</span> Timer</h1>
                <form
                    className="z-20 rounded-[20px] p-5 min-w-40 max-w-70 text-white border-solid border-2 border-white/[0.2] outline-none shadow-white shadow-sm backdrop-blur-sm"
                    onSubmit={e => e.preventDefault()}>
                    <h1 className="text-center text-4xl font-bold">Login</h1>
                    <div className="relative w-full h-10 my-5">
                        <input value={formData.email} onChange={(event) => setFormData(
                            { ...formData, email: event.target.value }
                        )} type="email" placeholder="user@example.com" required={true}
                        className="py-5 pr-5 pl-[45px] text-[20px] placeholder:text-white h-full w-full bg-transparent border-solid border-2 border-white/[0.2] outline-none rounded-[20px]" />
                        <UserCircleIcon
                            className="absolute left-[20px] top-[50%] translate-y-[-50%] h-[20px] w-[20px] text-white" />
                    </div>
                    <div className="relative w-full h-10 my-5">
                        <input type={visible ? "text" : "password"} onChange={(event) => setFormData(
                            { ...formData, password: event.target.value }
                        )} placeholder="Password" required={true}
                        className="py-5 pr-5 px-[45px] text-[20px] placeholder:text-white h-full w-full bg-transparent border-solid border-2 border-white/[0.2] outline-none rounded-[20px]" />
                        <LockClosedIcon
                            className="absolute left-[20px] top-[50%] translate-y-[-50%] h-[20px] w-[20px] text-white" />
                        {visible ? <EyeIcon
                            className="absolute right-[20px] top-[50%] translate-y-[-50%] h-[20px] w-[20px] text-white"
                            onClick={() => setVisible(!visible)} /> : <EyeSlashIcon
                            className="absolute right-[20px] top-[50%] translate-y-[-50%] h-[20px] w-[20px] text-white"
                            onClick={() => setVisible(!visible)} />}
                    </div>
                    <div>
                        <label className="text-[15px]"><input type="checkbox" className="mr-1" />Remember me</label>
                    </div>
                    <div className="items-center">
                        <button onClick={Submit}
                            className="my-3 w-full h-[45px] bg-white outline-none rounded-[20px] shadow-sm cursor-pointer text-black">Login
                        </button>
                    </div>
                    <div className="text-center text-[15px]">
                        {/* eslint-disable-next-line react/no-unescaped-entities */}
                        <p>Don't have an account? <Link to="../../Register"
                            className="font-bold hover:underline">Register</Link></p>
                    </div>
                </form>
            </div>
            <div className="dottedBg absolute top-8 left-8 w-32 h-32 z-1 max-sm:hidden" />
            <div className="dottedBg absolute bottom-4 right-4 w-[448px] h-72 z-1 max-lg:hidden" />
        </>
    );
};

export default LoginForm;
