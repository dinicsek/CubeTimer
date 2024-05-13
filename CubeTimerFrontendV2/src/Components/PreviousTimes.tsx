import { useState } from "react";

function PreviousTimes() {
    const [Timeys, setTimeys] = useState();


    return (<>
        <button onClick={event => {
            fetch("http://localhost:5123/Solve", {
                method: "GET",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${localStorage.getItem("token")}`
                },
            }).then((res) => {
                if (res.ok) {
                    return res.json();
                }
                throw new Error("Something went wrong");
            }).then((data) => {
                setTimeys(data);
            });
        }}>Sex
        </button>
        <p>{Timeys}</p>
    </>);
}

export default PreviousTimes;
