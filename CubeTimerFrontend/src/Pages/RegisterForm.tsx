// Date: 2023/01/03
// Desc: Login Form
import {EnvelopeIcon, EyeIcon, EyeSlashIcon, LockClosedIcon, UserCircleIcon, UserIcon} from "@heroicons/react/16/solid";
import {useState} from "react";
import '../index.css';
import '../assets/pexels-pixabay-417173.jpg';
import {Link, useNavigate} from "react-router-dom";

const RegisterForm = () => {
    const [visible, setVisible] = useState(false);
    const [pswdVisible, setPswdVisible] = useState(false);
    const [formData, setFormData] = useState({
        email: '',
        firstName: '',
        lastName: '',
        username: '',
        password: '',
        passwordConfirmation: ''
    });

    const navigate = useNavigate();

    function Submit() {
        if (formData.password !== formData.passwordConfirmation) {
            alert('Passwords do not match!');
            return false;
        } else {
            fetch("http://localhost:5123/Auth/register", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(formData),

            }).then((res) => {
                if (res.ok) {
                    navigate('/Login');
                } else {
                    throw new Error("Something went wrong");
                }
            }).catch((err) => {
                console.log(err);
            })
        }
    }

    //Dingus code below this line (I'm not sure if it works) -Dingus
    return (
        <>
            <div
                className='flex flex-col justify-center items-center h-screen w-screen bg-blue-600 font-mono m-0 p-0 box-border'>
                <h1 className='text-7xl text-white m-6 font-bold'><span className=''>Sharp</span> Timer</h1>
                <form
                    className='rounded-[20px] z-20 p-5 min-w-40 max-w-70 text-white border-solid border-2 border-white/[0.2] outline-none shadow-white shadow-sm backdrop-blur-sm'
                    onSubmit={e => e.preventDefault()}>
                    <h1 className='text-center text-4xl font-bold gap-0'>
                        Register</h1>
                    <div className='relative w-full h-10 my-5'>
                        <input value={formData.email} onChange={(event) => setFormData({
                            ...formData,
                            email: event.target.value
                        })} id='email' type='email' placeholder='user@example.com' required
                               className='py-5 pr-5 pl-[45px] text-[20px] placeholder:text-white h-full w-full bg-transparent border-solid border-2 border-white/[0.2] outline-none rounded-[20px]'/>
                        <EnvelopeIcon
                            className="absolute left-[20px] top-[50%] translate-y-[-50%] h-[20px] w-[20px] text-white"/>
                    </div>
                    <div className='flex flex-row relative w-full h-10 my-5'>
                        <div className='relative'>
                            <input value={formData.firstName} onChange={(event) => setFormData(
                                {...formData, firstName: event.target.value}
                            )}
                                   type='text' required placeholder='First name'
                                   className='grid-cols-1 mr-1 max-w-[250px] py-5 pr-5 pl-[45px] text-[20px] placeholder:text-white h-full w-full bg-transparent border-solid border-2 border-white/[0.2] outline-none rounded-[20px]'/>
                            <UserIcon
                                className="absolute left-[20px] top-[50%] translate-y-[-50%] h-[20px] w-[20px] text-white"/>
                        </div>
                        <div className='relative'>
                            <input value={formData.lastName} onChange={(event) => setFormData(
                                {...formData, lastName: event.target.value}
                            )} type='text' required placeholder='Last name'
                                   className='grid-cols-2 max-w-[250px] py-5 pr-5 pl-[45px] text-[20px] placeholder:text-white h-full w-full bg-transparent border-solid border-2 border-white/[0.2] outline-none rounded-[20px]'/>
                            <UserIcon
                                className="absolute left-[20px] top-[50%] translate-y-[-50%] h-[20px] w-[20px] text-white"/>
                        </div>
                    </div>
                    <div className='relative w-full h-10 my-5'>
                        <input value={formData.username} onChange={(event) => setFormData(
                            {...formData, username: event.target.value}
                        )} type='text' required placeholder='Username'
                               className='py-5 pr-5 pl-[45px] text-[20px] placeholder:text-white h-full w-full bg-transparent border-solid border-2 border-white/[0.2] outline-none rounded-[20px]'/>
                        <UserCircleIcon
                            className="absolute left-[20px] top-[50%] translate-y-[-50%] h-[20px] w-[20px] text-white"/>
                    </div>
                    <div className='relative w-full h-10 my-5'>
                        <input value={formData.password}
                               pattern='^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$'
                               type={visible ? "text" : "password"} onChange={(event) => setFormData(
                            {...formData, password: event.target.value}
                        )} placeholder='Password' required
                               className='py-5 pr-5 px-[45px] text-[20px] placeholder:text-white h-full w-full bg-transparent border-solid border-2 border-white/[0.2] outline-none rounded-[20px]'/>
                        <LockClosedIcon
                            className="absolute left-[20px] top-[50%] translate-y-[-50%] h-[20px] w-[20px] text-white"/>
                        {visible ? <EyeIcon
                            className="absolute right-[20px] top-[50%] translate-y-[-50%] h-[20px] w-[20px] text-white"
                            onClick={() => setVisible(!visible)}/> : <EyeSlashIcon
                            className="absolute right-[20px] top-[50%] translate-y-[-50%] h-[20px] w-[20px] text-white"
                            onClick={() => setVisible(!visible)}/>}
                    </div>
                    <div className='relative w-full h-10 my-5'>
                        <input pattern='^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$'
                               type={pswdVisible ? "text" : "password"} onChange={(event) => setFormData(
                            {...formData, passwordConfirmation: event.target.value}
                        )} placeholder='Confirm password' required
                               className='py-5 pr-5 px-[45px] text-[20px] placeholder:text-white h-full w-full bg-transparent border-solid border-2 border-white/[0.2] outline-none rounded-[20px]'/>
                        <LockClosedIcon
                            className="absolute left-[20px] top-[50%] translate-y-[-50%] h-[20px] w-[20px] text-white"/>
                        {pswdVisible ? <EyeIcon
                            className="absolute right-[20px] top-[50%] translate-y-[-50%] h-[20px] w-[20px] text-white"
                            onClick={() => setPswdVisible(!pswdVisible)}/> : <EyeSlashIcon
                            className="absolute right-[20px] top-[50%] translate-y-[-50%] h-[20px] w-[20px] text-white"
                            onClick={() => setPswdVisible(!pswdVisible)}/>}
                    </div>
                    <div className='items-center'>
                        <button onClick={Submit}
                                className='my-3 w-full h-[45px] bg-white outline-none rounded-[20px] shadow-sm cursor-pointer text-black'>Register
                        </button>
                    </div>
                    <div className='text-center text-[15px]'>
                        <p>Already have an account? <Link to='../../Login'
                                                          className='font-bold hover:underline'>Login</Link></p>
                    </div>
                </form>
            </div>
            <div className="dottedBg absolute top-8 left-8 w-32 h-32 z-1 max-sm:hidden"/>
            <div className="dottedBg absolute bottom-4 right-4 w-[448px] h-72 z-1 max-lg:hidden"/>
        </>
    );
}

export default RegisterForm;