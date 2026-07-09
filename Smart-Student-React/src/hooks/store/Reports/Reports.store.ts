import { create, StateCreator } from "zustand";
import { ApiResponse } from "../../../interfaces/ApiResponse";
import { ReportDataResponse } from "../../../interfaces/Reports/ReportDataResponse";
import ReportsService from "../../../services/Reports/ReportsService";
import { RequestReport } from "../../../interfaces/Reports/RequestReporT";
import { SaveCommentsRequest } from "../../../interfaces/Reports/SaveCommentsRequest";
import { RequestTeacherSubject } from "../../../interfaces/Reports/RequestTeacherSubject";
import { SelectedInterface } from "../../../interfaces/SelectedInterface";

interface ReportDataResponsestate {
    loading: boolean;
    reportData: ReportDataResponse;
    lastStudentId: number | null;
    base64Preview: string | null;
    subjectsByTeacher: SelectedInterface<number>[];
    subjectSelect: number;

    GetDataReport: (
        requestReport: RequestReport,
        forceRefres: boolean
    ) => Promise<ApiResponse<ReportDataResponse>>;
    GetReport: (
        requestReport: RequestReport
    ) => Promise<ApiResponse<ReportDataResponse>>;
    OnUpdateEvaluation: (
        requestReport: RequestReport
    ) => Promise<ApiResponse<number>>;
    OnUpdateCalification: (
        requestReport: RequestReport
    ) => Promise<ApiResponse<number>>;
    OnSaveComments: (
        request: SaveCommentsRequest
    ) => Promise<ApiResponse<number>>;
    OnDeleteEvaluation: (
        requestReport: RequestReport
    ) => Promise<ApiResponse<number>>;
    OnDeleteCalification: (
        requestReport: RequestReport
    ) => Promise<ApiResponse<number>>;
    GetSubjectsByTeacher(
        requestReport: RequestTeacherSubject
    ): Promise<ApiResponse<SelectedInterface<number>[]>>;
    updateEstadoVmPrinc: (indicatorId: number, newEstado: string) => void;
    clearEstadoVmPrinc: (indicatorId: number) => void;

    setSubjectSelect: (subjectSelect: number) => void;
}

const creatorReportDataResponse: StateCreator<ReportDataResponsestate> = (
    set
) => ({
    loading: true,
    reportData: {} as ReportDataResponse,
    lastStudentId: null,
    base64Preview: null,
    subjectsByTeacher: [],
    subjectSelect: 0,
    setSubjectSelect: (subjectSelect: number) =>
        set(() => ({
            subjectSelect: subjectSelect,
        })),
    GetDataReport: async (
        requestReport: RequestReport
    ): Promise<ApiResponse<ReportDataResponse>> => {
        try {
            set(() => ({ loading: true }));

            const dataReponse = await ReportsService.GetDataReport(requestReport);

            set((state) => ({
                ...state,
                reportData: dataReponse.data,
                lastStudentId: requestReport.idStudent,
                loading: false,
            }));

            return dataReponse;
        } catch (error) {
            console.log(error);
            set(() => ({ loading: false }));
            return {} as ApiResponse<ReportDataResponse>;
        }
    },
    GetReport: async (
        requestReport: RequestReport
    ): Promise<ApiResponse<ReportDataResponse>> => {
        try {
            // set(() => ({ loading: true, base64Preview: null }));

            const dataReponse = await ReportsService.GetReport(requestReport);
            // set((state) => ({
            //     ...state,
            //     loading: false,
            //     base64Preview: dataReponse.data.base64Preview,
            // }));

            return dataReponse;
        } catch (error) {
            console.log(error);
            set(() => ({ loading: false }));
            return {} as ApiResponse<ReportDataResponse>;
        }
    },
    GetSubjectsByTeacher: async (
        requestReport: RequestTeacherSubject
    ): Promise<ApiResponse<SelectedInterface<number>[]>> => {
        try {
            set(() => ({ loading: true, base64Preview: null }));

            const dataReponse = await ReportsService.GetSubjectsByTeacher(
                requestReport
            );
            const subjectSelect = dataReponse.data[0].id;
            set((state) => ({
                ...state,
                loading: false,
                subjectsByTeacher: dataReponse.data,
                subjectSelect,
            }));

            return dataReponse;
        } catch (error) {
            console.log(error);
            set(() => ({ loading: false }));
            return {} as ApiResponse<SelectedInterface<number>[]>;
        }
    },
    OnUpdateEvaluation: async (
        requestReport: RequestReport
    ): Promise<ApiResponse<number>> => {
        try {
            const dataReponse = await ReportsService.UpdateEvaluation(requestReport);
            return dataReponse;
        } catch (error) {
            console.log(error);
            set(() => ({ loading: false }));
            return {} as ApiResponse<number>;
        }
    },
    OnUpdateCalification: async (
        requestReport: RequestReport
    ): Promise<ApiResponse<number>> => {
        try {
            const dataReponse = await ReportsService.UpdateCalification(
                requestReport
            );
            return dataReponse;
        } catch (error) {
            console.log(error);
            set(() => ({ loading: false }));
            return {} as ApiResponse<number>;
        }
    },
    OnSaveComments: async (
        request: SaveCommentsRequest
    ): Promise<ApiResponse<number>> => {
        try {
            const dataReponse = await ReportsService.SaveComments(request);
            return dataReponse;
        } catch (error) {
            console.log(error);
            set(() => ({ loading: false }));
            return {} as ApiResponse<number>;
        }
    },
    OnDeleteEvaluation: async (
        requestReport: RequestReport
    ): Promise<ApiResponse<number>> => {
        try {
            const dataReponse = await ReportsService.Delete(requestReport);
            return dataReponse;
        } catch (error) {
            console.log(error);
            set(() => ({ loading: false }));
            return {} as ApiResponse<number>;
        }
    },
    OnDeleteCalification: async (
        requestReport: RequestReport
    ): Promise<ApiResponse<number>> => {
        try {
            const dataReponse = await ReportsService.DeleteCalification(
                requestReport
            );
            return dataReponse;
        } catch (error) {
            console.log(error);
            set(() => ({ loading: false }));
            return {} as ApiResponse<number>;
        }
    },
    updateEstadoVmPrinc: (indicatorId: number, newEstado: string) => {
        set((state) => {
            const updatedEvaluationsDetails = state.reportData.evaluationsDetails.map(
                (detail) => ({
                    ...detail,
                    indicators: detail.indicators.map((item) =>
                        item.indicator.id === indicatorId
                            ? { ...item, estado: newEstado }
                            : item
                    ),
                })
            );

            return {
                ...state,
                reportData: {
                    ...state.reportData,
                    evaluationsDetails: updatedEvaluationsDetails,
                },
            };
        });
    },
    clearEstadoVmPrinc: (indicatorId: number) => {
        set((state) => {
            const updatedEvaluationsDetails = state.reportData.evaluationsDetails.map(
                (detail) => ({
                    ...detail,
                    indicators: detail.indicators.map((item) =>
                        item.indicator.id === indicatorId ? { ...item, estado: "" } : item
                    ),
                })
            );

            return {
                ...state,
                reportData: {
                    ...state.reportData,
                    evaluationsDetails: updatedEvaluationsDetails,
                },
            };
        });
    },
});

export const useReportDataStore = create<ReportDataResponsestate>()(
    creatorReportDataResponse
);
