import {useEffect} from 'react';
import LoginForm from "./LoginForm.tsx";
// import RegisterForm from "./RegisterForm.tsx";
// import Timer from "./Timer.tsx";
function App() {
    useEffect(() => {
            console.log(
                "   _____ _                   _______ _                     \n  / ____| |                 |__   __(_)                    \n | (___ | |__   __ _ _ __ _ __ | |   _ _ __ ___   ___ _ __ \n  \\___ \\| '_ \\ / _` | '__| '_ \\| |  | | '_ ` _ \\ / _ \\ '__|\n  ____) | | | | (_| | |  | |_) | |  | | | | | | |  __/ |   \n |_____/|_| |_|\\__,_|_|  | .__/|_|  |_|_| |_| |_|\\___|_|   \n                         | |                               \n"
            );
    }, []);
  return (
    <>
        <div className='flex flex-col justify-center items-center h-screen w-screen bg-blue-600 font-mono m-0 p-0 box-border'>
            <h1 className='text-7xl text-white m-6 font-bold'><span className=''>Sharp</span> Timer</h1>
            {/*<Timer />*/}
            <LoginForm />
            {/*<RegisterForm />*/}
        </div>
        <div className="dottedBg absolute top-8 left-8 w-32 h-32 z-1 max-sm:hidden" />
        <div className="dottedBg absolute bottom-4 right-4 w-[448px] h-72 z-1 max-lg:hidden"/>
    </>
  )
}

export default App
