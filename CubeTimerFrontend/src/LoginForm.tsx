// Date: 2023/01/03
// Desc: Login Form
import { LockClosedIcon, UserCircleIcon, EyeIcon, EyeSlashIcon } from "@heroicons/react/16/solid";
import { useState} from "react";
import './index.css';
import './assets/pexels-pixabay-417173.jpg';
const LoginForm = () => {
    const [visible, setVisible] = useState(false);
    const [formData, setFormData] = useState({
        email: '',
        password: ''
    })


    return (
      <form className='z-20 rounded-[20px] p-5 min-w-40 max-w-70 text-white border-solid border-2 border-white/[0.2] outline-none shadow-white shadow-sm backdrop-blur-md'>
          <h1 className='text-center text-4xl font-bold'>Login</h1>
          <div className='relative w-full h-10 my-5'>
              <input value={formData.email}  onChange={(event) => setFormData(
              {...formData, email: event.target.value}
                  )} type='email' placeholder='user@example.com' required className='py-5 pr-5 pl-[45px] text-[20px] placeholder:text-white h-full w-full bg-transparent border-solid border-2 border-white/[0.2] outline-none rounded-[20px]'/>
              <UserCircleIcon className="absolute left-[20px] top-[50%] translate-y-[-50%] h-[20px] w-[20px] text-white" />
          </div>
          <div className='relative w-full h-10 my-5'>
              <input type={visible ? "text" : "password"} onChange={(event) => setFormData(
                  {...formData, password: event.target.value}
              )} placeholder='Password' required className='py-5 pr-5 px-[45px] text-[20px] placeholder:text-white h-full w-full bg-transparent border-solid border-2 border-white/[0.2] outline-none rounded-[20px]'/>
              <LockClosedIcon className="absolute left-[20px] top-[50%] translate-y-[-50%] h-[20px] w-[20px] text-white" />
              {visible ? <EyeIcon className="absolute right-[20px] top-[50%] translate-y-[-50%] h-[20px] w-[20px] text-white" onClick={() => setVisible(!visible)}/> : <EyeSlashIcon className="absolute right-[20px] top-[50%] translate-y-[-50%] h-[20px] w-[20px] text-white" onClick={() => setVisible(!visible)}/>}
          </div>
          <div>
              <label className='text-[15px]'><input type='checkbox' className='mr-1'/>Remember me</label>
          </div>
          <div className='items-center'>
                <button type='submit' className='my-3 w-full h-[45px] bg-white outline-none rounded-[20px] shadow-sm cursor-pointer text-black'>Login</button>
          </div>
          <div className='text-center text-[15px]'>
                <p>Don't have an account? <a className='font-bold hover:underline'>Register</a></p>
          </div>
      </form>

  );
}

export default LoginForm;