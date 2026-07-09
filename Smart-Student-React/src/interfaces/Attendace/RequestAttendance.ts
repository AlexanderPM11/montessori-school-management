export interface RequestAttendance {
    idRoom: number;
    dayWeek: string;
    idStudent: number;
    status: string;
    observations?: string; // El signo ? indica que es opcional
}
export class RequestAttendanceConvert {
    public static toRequestAttendance(json: string | object): RequestAttendance {
        if (typeof json === "string") {
            return JSON.parse(json);
        }
        return json as RequestAttendance;
    }
}
