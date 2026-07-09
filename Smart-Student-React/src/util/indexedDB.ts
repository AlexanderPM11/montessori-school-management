// utils/indexedDB.ts
import localForage from 'localforage';

export const setItemDB = async <T>(key: string, value: T) => {
    try {
        await localForage.setItem(key, value);
    } catch (error) {
        console.error(`Error saving ${key} in IndexedDB`, error);
    }
};

export const getItemDB = async <T>(key: string): Promise<T | null> => {
    try {
        const value = await localForage.getItem<T>(key);
        return value ?? null;
    } catch (error) {
        console.error(`Error reading ${key} from IndexedDB`, error);
        return null;
    }
};

export const removeItemDB = async (key: string) => {
    try {
        await localForage.removeItem(key);
    } catch (error) {
        console.error(`Error removing ${key} from IndexedDB`, error);
    }
};
