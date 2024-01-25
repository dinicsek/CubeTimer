import { Cog6ToothIcon } from "@heroicons/react/16/solid";
import { useState } from "react";

function Settings() {
    const [expanded, setExpanded] = useState(false);
    return (
        <div>
            <Cog6ToothIcon />
        </div>
    );
}
