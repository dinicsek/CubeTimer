import { Cog6ToothIcon } from "@heroicons/react/16/solid";
import * as Popover from "@radix-ui/react-popover";
import * as RadioGroup from "@radix-ui/react-radio-group";
import { SettingsContext } from "../Contexts/SettingsContext.tsx";
import { useContext } from "react";
import Logout from "./Logout.tsx";


function Settings() {
    const settings = useContext(SettingsContext);

    return (
        <Popover.Root>
            <Popover.Trigger asChild={true}>
                <Cog6ToothIcon />
            </Popover.Trigger>
            <Popover.Anchor />
            <Popover.Portal>
                <Popover.Content className="mt-0">
                    <div
                        className="min-w-40 px-5 z-100 m-4 py-1 rounded-[20px] border-solid border-2 border-white/[0.2] outline-none shadow-white shadow-sm backdrop-blur-sm absolute -translate-x-[100%] text-balance">
                        <p className="font-bold w-1/2 whitespace-nowrap">Scramble Length</p>
                        <input
                            className="w-[40%] bg-transparent border-solid border-2 border-white/[0.2] outline-none rounded-[20px] pl-2"
                            type="number"
                            value={settings.length}
                            onChange={event => {
                                settings.setLength(+event.currentTarget.value);
                            }} />
                        <p className="font-bold w-1/2 whitespace-nowrap">Inspection Enabled</p>
                        <input
                            className="w-[40%] bg-transparent border-solid border-2 border-white/[0.2] outline-none rounded-[20px] pl-2"
                            type="checkbox"
                            checked={settings.inspectionEnabled}
                            onChange={event => {
                                settings.setInspectionEnabled(!settings.inspectionEnabled);
                            }} />
                        <p className="font-bold w-1/2 whitespace-nowrap">Event</p>
                        <RadioGroup.Root />
                        <p className="font-bold w-[60%] whitespace-nowrap">Cube Type</p>
                        <input
                            className="w-[70%] bg-transparent border-solid border-2 border-white/[0.2] outline-none rounded-[20px] pl-2"
                            type="text"
                            value={settings.cubeType}
                            onChange={event => {
                                settings.setCubeType(event.currentTarget.value);
                            }} />
                    </div>
                    <Logout />
                </Popover.Content>
            </Popover.Portal>
        </Popover.Root>
    );
}

export default Settings;
