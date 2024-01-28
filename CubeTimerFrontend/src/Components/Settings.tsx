import { Cog6ToothIcon } from "@heroicons/react/16/solid";
import * as Popover from "@radix-ui/react-popover";

function Settings() {
    return (
        <Popover.Root>
            <Popover.Trigger>
                <Cog6ToothIcon />
            </Popover.Trigger>
            <Popover.Anchor />
            <Popover.Portal>
                <Popover.Content>
                    <Popover.Close />
                    <Popover.Arrow />
                </Popover.Content>
            </Popover.Portal>
        </Popover.Root>
    );
}

export default Settings;
