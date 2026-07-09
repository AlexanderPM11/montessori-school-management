import { create, StateCreator } from "zustand";
import { ApiResponse } from "../../../interfaces/ApiResponse";
import ReportAttendanceService from "../../../services/Attendance/ReportAttendanceService";

interface ReportAttendanceState {
    // Properties
    loading: boolean;
    base64ReportAttendace: string;
    base64ReportAttendaceStudent: string;

    // Actions
    getPdf: (
        idRoom: number,
        date: string
    ) => Promise<ApiResponse<string>>;
    GetPdfStudent: (
        idStudent: number,
        date: string
    ) => Promise<ApiResponse<string>>;
}

const creatorReportAttendance: StateCreator<ReportAttendanceState> = (set) => ({
    loading: true,
    base64ReportAttendaceStudent: {} as string,
    base64ReportAttendace: {} as string,

    getPdf: async (idRoom: number, date: string) => {
        try {
            set({ loading: true });
            const dataReponse = await ReportAttendanceService.GetPdf(idRoom, date);
            set({ base64ReportAttendace: dataReponse.data, loading: false });
            return dataReponse;
        } catch (error) {
            console.log(error);
            set({ loading: false });
            return {} as ApiResponse<string>;
        }
    },
    GetPdfStudent: async (idStudent: number, date: string) => {
        try {
            set({ loading: true });
            const dataReponse = await ReportAttendanceService.GetPdfStudent(idStudent, date);
            set({ base64ReportAttendaceStudent: dataReponse.data, loading: false });
            return dataReponse;
        } catch (error) {
            console.log(error);
            set({ loading: false });
            return {} as ApiResponse<string>;
        }
    },
});

export const useReportAttendanceStore = create<ReportAttendanceState>()(
    creatorReportAttendance
);
