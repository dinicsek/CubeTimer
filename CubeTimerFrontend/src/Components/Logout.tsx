function Logout() {
    return (
        <button onClick={RemoveFromLocalStorage}>Logout</button>
    );
}

function RemoveFromLocalStorage() {
    localStorage.removeItem("token");
    location.reload();
}

export default Logout;
