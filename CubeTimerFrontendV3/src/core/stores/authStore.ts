import {createJSONStorage, persist, PersistOptions} from "zustand/middleware/persist";
import {StateCreator} from "zustand/vanilla";
import {create} from "zustand/react";

export interface AuthState {
    accessToken?: string;
    setAccessToken: (token?: string) => void;
}

type SettingsStorePersist = (
    config: StateCreator<AuthState>,
    options: PersistOptions<AuthState>
) => StateCreator<AuthState>


export const useAuthStore = create<AuthState>(
    (persist as unknown as SettingsStorePersist)(
        // @ts-ignore
        (set, get) => ({
            accessToken: undefined,
            setAccessToken: (accessToken?: string) => set({accessToken}),
        }),
        {
            name: 'auth-storage',
            storage: createJSONStorage(() => sessionStorage),
        })
    );