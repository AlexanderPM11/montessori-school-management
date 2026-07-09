import { create, StateCreator } from "zustand";
import { ApiResponse } from "../../../interfaces/ApiResponse";
import GeneralService from "../../../services/General/GeneralService";
import { GendersCivStatuNacLevEducRelati } from "../../../interfaces/General/GendersCivStatuNacLevEducRelati";
import { SelectedInterface } from "../../../interfaces/SelectedInterface";
import { GeneralUserRoles } from "../../../interfaces/General/GeneralUserRoles";

interface GeneralState {
    // Properties
    loading: boolean;
    isCreating: boolean;
    civilStatus?: SelectedInterface<number>[] | null;
    nationality?: SelectedInterface<number>[] | null;
    educationalLevel?: SelectedInterface<number>[] | null;
    relationship?: SelectedInterface<number>[] | null;
    professions?: SelectedInterface<number>[] | null;
    genders?: SelectedInterface<number>[] | null;
    grades?: SelectedInterface<number>[] | null;
    levels: SelectedInterface<number>[];
    provices: SelectedInterface<number>[];
    periods: SelectedInterface<string>[];
    defaultperiod: string;
    teachers: SelectedInterface<string>[];
    teachersByRoom: SelectedInterface<string>[];
    idRoomStore: number;
    generalUserRoles: GeneralUserRoles;
    daysOfWeek: string[];

    // Actions
    getTeacher: () => Promise<ApiResponse<SelectedInterface<string>[]>>;
    geDaysOfWeek: () => Promise<ApiResponse<string[]>>;
    GetGeneralsUserRoles: () => Promise<ApiResponse<GeneralUserRoles>>;
    getTeachersByIdRoom: (
        idRoom: number
    ) => Promise<ApiResponse<SelectedInterface<string>[]>>;
    getLevels: () => Promise<ApiResponse<SelectedInterface<number>[]>>;
    getProvices: () => Promise<ApiResponse<SelectedInterface<number>[]>>;
    getPeriods: (
        isPrimaria: boolean
    ) => Promise<ApiResponse<SelectedInterface<string>[]>>;
    onGetData: () => Promise<ApiResponse<GendersCivStatuNacLevEducRelati>>;
    setIscreating: (value: boolean) => void;
    setIsLoading: (value: boolean) => void;
}

const creatorGeneral: StateCreator<GeneralState> = (set, get) => ({
    isCreating: false,
    loading: false,
    civilStatus: null,
    nationality: null,
    educationalLevel: null,
    relationship: null,
    professions: null,
    genders: null,
    levels: [],
    provices: [],
    periods: [],
    defaultperiod: "",
    teachers: [],
    teachersByRoom: [],
    daysOfWeek: [],
    idRoomStore: 0,
    generalUserRoles: {} as GeneralUserRoles,

    //
    setIscreating: (value: boolean) => {
        set(() => ({ isCreating: value }));
    },
    setIsLoading: (value: boolean) => {
        set(() => ({ loading: value }));
    },
    onGetData: async (): Promise<
        ApiResponse<GendersCivStatuNacLevEducRelati>
    > => {
        try {
            set(() => ({ loading: true }));

            // Realizar la llamada a la API
            const data = await GeneralService.GetGendersCivStatuNacLevEducRelati();
            set((state) => {
                state.loading = false;
                if (data.result) {
                    state.civilStatus = data.data.civilStatus;
                    state.nationality = data.data.nationality;
                    state.educationalLevel = data.data.educationalLevel;
                    state.relationship = data.data.relationship;
                    state.professions = data.data.professions;
                    state.genders = data.data.genders;
                    state.grades = data.data.grades;
                    state.levels = data.data.levels;
                }

                return state;
            });
            set(() => ({ loading: false }));
            return data;
        } catch (error) {
            console.error("Error during login:", error);
            set(() => ({ loading: false }));
            return {} as ApiResponse<GendersCivStatuNacLevEducRelati>;
        }
    },
    getLevels: async (): Promise<ApiResponse<SelectedInterface<number>[]>> => {
        try {
            // Realizar la llamada a la API
            const data = await GeneralService.GetLevels();
            set((state) => {
                state.loading = false;
                if (data.result) {
                    state.levels = data.data;
                }

                return state;
            });
            return data;
        } catch (error) {
            console.error("Error during login:", error);
            return {} as ApiResponse<SelectedInterface<number>[]>;
        }
    },
    getProvices: async (): Promise<ApiResponse<SelectedInterface<number>[]>> => {
        try {
            // Realizar la llamada a la API
            const data = await GeneralService.GetProvices();
            set((state) => {
                state.loading = false;
                if (data.result) {
                    state.provices = data.data;
                }

                return state;
            });
            return data;
        } catch (error) {
            console.error("Error during login:", error);
            return {} as ApiResponse<SelectedInterface<number>[]>;
        }
    },
    getPeriods: async (
        isPrimaria: boolean = false
    ): Promise<ApiResponse<SelectedInterface<string>[]>> => {
        try {
            // const { periods } = get();

            // Solo hacemos el request si periods está vacío o no inicializado
            // if (periods && periods.length > 0) {
            //     return {
            //         result: true,
            //         data: periods,
            //         messages: ["Datos cacheados"],
            //     } as ApiResponse<SelectedInterface<string>[]>;
            // }

            const data = await GeneralService.GetPeriods(isPrimaria);
            set((state) => {
                state.loading = false;
                state.defaultperiod = data.data[0].text;
                if (data.result) {
                    state.periods = data.data;
                }
                return state;
            });

            return data;
        } catch (error) {
            console.error("Error during getPeriods:", error);
            return {} as ApiResponse<SelectedInterface<string>[]>;
        }
    },

    getTeacher: async (): Promise<ApiResponse<SelectedInterface<string>[]>> => {
        try {
            // Realizar la llamada a la API
            let dataReponse: ApiResponse<SelectedInterface<string>[]> = {
                data: [],
                result: false,
            };
            const storeteachers = get().teachers;

            if (!Array.isArray(storeteachers) || storeteachers.length === 0) {
                set(() => ({ loading: true }));
                dataReponse = await GeneralService.GetTeachers();
            } else {
                dataReponse = { data: storeteachers, result: true };
            }

            set((state) => {
                if (dataReponse.result) {
                    state.teachers = dataReponse.data;
                }
                return state;
            });
            return dataReponse;
        } catch (error) {
            console.log("Ocurrio  un error" + error);

            return {} as ApiResponse<SelectedInterface<string>[]>;
        }
    },
    GetGeneralsUserRoles: async (): Promise<ApiResponse<GeneralUserRoles>> => {
        try {
            // Realizar la llamada a la API
            let dataReponse: ApiResponse<GeneralUserRoles> = {
                data: {} as GeneralUserRoles,
                result: false,
            };
            const storeUseRoles = get().generalUserRoles;
            if (!Array.isArray(storeUseRoles) || storeUseRoles.length === 0) {
                set(() => ({ loading: true }));
                dataReponse = await GeneralService.GetGeneralsUserRoles();
            } else {
                dataReponse = { data: storeUseRoles, result: true };
            }

            set((state) => {
                if (dataReponse.result) {
                    state.generalUserRoles = dataReponse.data;
                }
                return state;
            });
            return dataReponse;
        } catch (error) {
            console.log("Ocurrio  un error" + error);

            return {} as ApiResponse<GeneralUserRoles>;
        }
    },
    getTeachersByIdRoom: async (
        idRoom: number
    ): Promise<ApiResponse<SelectedInterface<string>[]>> => {
        try {
            // Realizar la llamada a la API
            let dataReponse: ApiResponse<SelectedInterface<string>[]> = {
                data: [],
                result: false,
            };
            const storeteachersByRoom = get().teachersByRoom;
            const idRoomStore = get().idRoomStore;

            if (
                !Array.isArray(storeteachersByRoom) ||
                storeteachersByRoom.length === 0 ||
                idRoom != idRoomStore
            ) {
                set(() => ({ loading: true }));
                dataReponse = await GeneralService.GetTeachersByIdRoom(idRoom);
            } else {
                dataReponse = { data: storeteachersByRoom, result: true };
            }

            set((state) => {
                if (dataReponse.result) {
                    state.teachersByRoom = dataReponse.data;
                    state.idRoomStore = idRoom;
                }
                return state;
            });
            return dataReponse;
        } catch (error) {
            console.log("Ocurrio  un error" + error);

            return {} as ApiResponse<SelectedInterface<string>[]>;
        }
    },
    geDaysOfWeek: async (): Promise<ApiResponse<string[]>> => {
        try {
            // Realizar la llamada a la API
            let dataReponse: ApiResponse<string[]> = {
                data: [],
                result: false,
            };
            const storeDaysOfWeek = get().daysOfWeek;

            if (!Array.isArray(storeDaysOfWeek) || storeDaysOfWeek.length === 0) {
                set(() => ({ loading: true }));
                dataReponse = await GeneralService.GetDaysOfWeek();
            } else {
                dataReponse = { data: storeDaysOfWeek, result: true };
            }

            set((state) => {
                if (dataReponse.result) {
                    state.daysOfWeek = dataReponse.data;
                }
                return state;
            });
            return dataReponse;
        } catch (error) {
            console.log("Ocurrio  un error" + error);

            return {} as ApiResponse<string[]>;
        }
    },
});

export const useGeneralStore = create<GeneralState>()(creatorGeneral);
