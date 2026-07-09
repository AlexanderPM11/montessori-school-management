import { AuditableBaseModel } from "../AuditableBaseModel";



export interface AttendanceSummaryViewModel extends AuditableBaseModel {
    attendances: AttendanceViewModel[];
    presentCount: number;
    delayCount: number;
    absentCount: number;
    excuseCount: number;
    totalCount: number;
}
export class AttendanceSummaryViewModelConvert {
    public static toAttendanceSummaryViewModel(json: string | object): AttendanceSummaryViewModel {
        if (typeof json === "string") {
            return JSON.parse(json);
        }
        return json as AttendanceSummaryViewModel;
    }
}



export interface AttendanceViewModel extends AuditableBaseModel {
    date: Date;
    observation?: string;
    isPresent?: boolean;
    isDelay?: boolean;
    isAbsent?: boolean;
    isExcuse?: boolean;

    // Custom
    fullNameStudent?: string;
    idRoom: number;
    idStudent: number;
    idSuject?: number;
    numberList: number;
}
