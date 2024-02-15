import { Cog6ToothIcon } from "@heroicons/react/16/solid";
import * as Popover from "@radix-ui/react-popover";
import { SettingsContext } from "../Contexts/SettingsContext.tsx";
import { useContext } from "react";
import * as RadioGroup from "@radix-ui/react-radio-group";


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
                        <p className="font-bold w-1/2">Length</p>
                        <input
                            className="w-[40%] bg-transparent border-solid border-2 border-white/[0.2] outline-none rounded-[20px] pl-2"
                            type="number"
                            defaultValue={settings.length}
                            onChange={event => {
                                settings.setLength(+event.currentTarget.value);
                            }} />
                        <p className="font-bold w-1/2">Inspection</p>
                        <input className="w-1/6 rounded-[20px] text-center" type="checkbox"
                            defaultChecked={settings.inspectionEnabled} onChange={event => {
                                settings.setInspectionEnabled(event.currentTarget.checked);
                            }} />
                        <p className="font-bold w-1/2">Event</p>
                        <div className="flex items-center">
                            <RadioGroup.Root defaultValue="ThreeByThree"
                                aria-label="View density" className="flex-row">
                                <RadioGroup.Item value="TwoByTwo" id="tbt"
                                    className="bg-white w-[12px] h-[12px] rounded-[20px] ">
                                    <RadioGroup.Indicator asChild={true}
                                        className="flex items-center justify-center w-full h-full relative after:content-[''] after:block after:w-[11px] after:h-[11px] after:rounded-[50%] after:bg-violet11" />
                                </RadioGroup.Item>
                                <label htmlFor="tbt">Two By Two</label>
                                <br />
                                <RadioGroup.Item value="ThreeByThree"
                                    className="bg-white w-[12px] h-[12px] rounded-full shadow-[0_2px_10px] shadow-blackA4 hover:bg-violet3 focus:shadow-[0_0_0_2px] focus:shadow-black outline-none cursor-default">
                                    <RadioGroup.Indicator
                                        className="flex items-center justify-center w-full h-full relative after:content-[''] after:block after:w-[11px] after:h-[11px] after:rounded-[50%] after:bg-violet11" />
                                </RadioGroup.Item>
                                <label>Three By Three</label>
                                <br />
                                <RadioGroup.Item value="FourByFour"
                                    className="bg-white w-[12px] h-[12px] rounded-full shadow-[0_2px_10px] shadow-blackA4 hover:bg-violet3 focus:shadow-[0_0_0_2px] focus:shadow-black outline-none cursor-default">
                                    <RadioGroup.Indicator
                                        className="flex items-center justify-center w-full h-full relative after:content-[''] after:block after:w-[11px] after:h-[11px] after:rounded-[50%] after:bg-violet11" />
                                </RadioGroup.Item>
                                <label>Four By Four</label>
                                <br />
                                <RadioGroup.Item value="FiveByFive"
                                    className="bg-white w-[12px] h-[12px] rounded-full shadow-[0_2px_10px] shadow-blackA4 hover:bg-violet3 focus:shadow-[0_0_0_2px] focus:shadow-black outline-none cursor-default">
                                    <RadioGroup.Indicator
                                        className="flex items-center justify-center w-full h-full relative after:content-[''] after:block after:w-[11px] after:h-[11px] after:rounded-[50%] after:bg-violet11" />
                                </RadioGroup.Item>
                                <label>Five By Five</label>
                                <br />
                                <RadioGroup.Item value="Pyraminx"
                                    className="bg-white w-[12px] h-[12px] rounded-full shadow-[0_2px_10px] shadow-blackA4 hover:bg-violet3 focus:shadow-[0_0_0_2px] focus:shadow-black outline-none cursor-default">
                                    <RadioGroup.Indicator
                                        className="flex items-center justify-center w-full h-full relative after:content-[''] after:block after:w-[11px] after:h-[11px] after:rounded-[50%] after:bg-violet11" />
                                </RadioGroup.Item>
                                <label>Pyraminx</label>
                            </RadioGroup.Root>
                        </div>
                        <p className="font-bold w-[60%] text-balance">Cube Type</p>
                        <input
                            className="w-[70%] bg-transparent border-solid border-2 border-white/[0.2] outline-none rounded-[20px] pl-2"
                            type="text"
                            defaultValue={settings.cubeType}
                            onChange={event => {
                                settings.setCubeType(event.currentTarget.value);
                            }} />
                    </div>
                </Popover.Content>
            </Popover.Portal>
        </Popover.Root>
    );
}

export default Settings;
