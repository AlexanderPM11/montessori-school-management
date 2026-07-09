import { create, StateCreator } from "zustand";
import { ApiResponse } from "../../../interfaces/ApiResponse";
import { AttendanceSummaryViewModel } from "../../../interfaces/Attendace/AttendanceSummaryViewModel";
import AttendaceService from "../../../services/Attendance/AttendanceService";
import { RequestAttendance } from "../../../interfaces/Attendace/RequestAttendance";

interface AttendaceState {
    // Properties
    loading: boolean;
    attendacesInfo: AttendanceSummaryViewModel;

    // Actions
    getAttendanceInfo: (
        idRoom: number,
        dayWeek: string
    ) => Promise<ApiResponse<AttendanceSummaryViewModel>>;
    onUpdateAttendance: (
        request: RequestAttendance
    ) => Promise<ApiResponse<string>>;
    onMakeAllPresent: (
        request: RequestAttendance
    ) => Promise<ApiResponse<string>>;
    handleStatusChange: (
        index: number,
        idStudent: number,
        idRoom: number,
        dayWeek: string,
        status: string
    ) => void;
    clearStatus: (index: number, idAttendance: number) => void;
    markAllAsPresent: (request: RequestAttendance) => void;
}

const creatorAttendace: StateCreator<AttendaceState> = (set) => ({
    loading: true,
    attendacesInfo: {} as AttendanceSummaryViewModel,

    getAttendanceInfo: async (idRoom: number, dayWeek: string) => {
        try {
            set({ loading: true });
            const dataReponse = await AttendaceService.GetAttendanceInfo(
                idRoom,
                dayWeek
            );
            set({ attendacesInfo: dataReponse.data, loading: false });
            return dataReponse;
        } catch (error) {
            console.log(error);
            set({ loading: false });
            return {} as ApiResponse<AttendanceSummaryViewModel>;
        }
    },

    onUpdateAttendance: async (request: RequestAttendance) => {
        try {
            set({ loading: true });
            const dataReponse = await AttendaceService.UpdateAttendance(request);
            set({ loading: false });
            return dataReponse;
        } catch (error) {
            console.log(error);
            set({ loading: false });
            return {} as ApiResponse<string>;
        }
    },


    onMakeAllPresent: async (request: RequestAttendance) => {
        try {
            set({ loading: true });
            const dataReponse = await AttendaceService.MakeAllPresent(request);
            set({ loading: false });
            return dataReponse;
        } catch (error) {
            console.log(error);
            set({ loading: false });
            return {} as ApiResponse<string>;
        }
    },

    handleStatusChange: (
        index: number,
        idStudent: number,
        idRoom: number,
        dayWeek: string,
        status: string
    ) => {
        const request = {
            idRoom: idRoom,
            dayWeek: dayWeek,
            idStudent: idStudent,
            status: status,
        };
        AttendaceService.UpdateAttendance(request);

        set((state) => {
            const updated = [...state.attendacesInfo.attendances];
            updated[index] = {
                ...updated[index],
                isPresent: false,
                isDelay: false,
                isAbsent: false,
                isExcuse: false,
                [status]: true,
            };

            const summary = {
                presentCount: updated.filter((a) => a.isPresent).length,
                delayCount: updated.filter((a) => a.isDelay).length,
                absentCount: updated.filter((a) => a.isAbsent).length,
                excuseCount: updated.filter((a) => a.isExcuse).length,
            };

            return {
                attendacesInfo: {
                    ...state.attendacesInfo,
                    attendances: updated,
                    ...summary,
                },
            };
        });
    },

    clearStatus: (index: number, idAttendance: number) => {
        AttendaceService.Delete(idAttendance);
        set((state) => {
            const updated = [...state.attendacesInfo.attendances];
            updated[index] = {
                ...updated[index],
                isPresent: false,
                isDelay: false,
                isAbsent: false,
                isExcuse: false,
            };

            const summary = {
                presentCount: updated.filter((a) => a.isPresent).length,
                delayCount: updated.filter((a) => a.isDelay).length,
                absentCount: updated.filter((a) => a.isAbsent).length,
                excuseCount: updated.filter((a) => a.isExcuse).length,
            };

            return {
                attendacesInfo: {
                    ...state.attendacesInfo,
                    attendances: updated,
                    ...summary,
                },
            };
        });
    },

    markAllAsPresent: (request: RequestAttendance) => {
        AttendaceService.MakeAllPresent(request)
        set((state) => {
            const updated = state.attendacesInfo.attendances.map((student) => ({
                ...student,
                isPresent: true,
                isDelay: false,
                isAbsent: false,
                isExcuse: false,
            }));

            return {
                attendacesInfo: {
                    ...state.attendacesInfo,
                    attendances: updated,
                    presentCount: updated.length,
                    delayCount: 0,
                    absentCount: 0,
                    excuseCount: 0,
                },
            };
        });
    },
});

export const useAttendaceStore = create<AttendaceState>()(creatorAttendace);
